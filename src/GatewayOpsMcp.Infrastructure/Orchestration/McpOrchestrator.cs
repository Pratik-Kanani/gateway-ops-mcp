using System.Diagnostics;
using GatewayOpsMcp.Core.Enums;
using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;
using GatewayOpsMcp.Core.Orchestration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace GatewayOpsMcp.Infrastructure.Orchestration;

public class McpOrchestrator : IMcpOrchestrator
{
    private readonly IToolRegistry _registry;
    private readonly IToolResolver _resolver;
    private readonly IPolicyEngine _policy;
    private readonly IPendingActionService _pendingActionService;
    private readonly IParameterExtractionService _extractor;
    private readonly IValidationService _validator;
    private readonly IAuditService _audit;
    private readonly IIdempotencyService _idempotency;
    private readonly IRequestHashService _hashService;
    private readonly IRateLimitService _rateLimit;
    private readonly ExecutionSettings _executionSettings;
    private readonly IExecutionLockService _lockService;
    private readonly IExecutionLockKeyBuilder _lockKeyBuilder;
    private readonly IMetricsService _metrics;
    private readonly IToolCompatibilityService _compatibility;

    public McpOrchestrator(
        IToolRegistry registry,
        IToolResolver resolver,
        IPolicyEngine policy,
        IPendingActionService pendingActionService,
        IParameterExtractionService extractor,
        IValidationService validator,
        IAuditService audit,
        IIdempotencyService idempotency,
        IRequestHashService hashService,
        IRateLimitService rateLimit,
        IOptions<ExecutionSettings> executionOptions,
        IExecutionLockService lockService,
        IExecutionLockKeyBuilder lockKeyBuilder,
        IMetricsService metrics,
        IToolCompatibilityService compatibility)
    {
        _registry = registry;
        _resolver = resolver;
        _policy = policy;
        _pendingActionService = pendingActionService;
        _extractor = extractor;
        _validator = validator;
        _audit = audit;
        _idempotency = idempotency;
        _hashService = hashService;
        _rateLimit = rateLimit;
        _executionSettings = executionOptions.Value;
        _lockService = lockService;
        _lockKeyBuilder = lockKeyBuilder;
        _metrics = metrics;
        _compatibility = compatibility;
    }

    // ENTRY

    public async Task<McpResponse> HandleAsync(
        McpRequest request,
        RequestContext context)
    {

        if (request.Confirm)
        {
            return await HandleConfirmationAsync(
                request,
                context);
        }

        var resolution = _resolver.Resolve(request.Input);

        if (resolution.IsAmbiguous)
        {
            await AuditAsync(
                context,
                "UNKNOWN",
                "AMBIGUOUS_TOOL_RESOLUTION",
                reason:
                    $"Candidates={string.Join(", ", resolution.CandidateTools)}");

            return new McpResponse
            {
                Response =
                    "Multiple possible actions detected. Please clarify.",

                Data = new
                {
                    candidates = resolution.CandidateTools
                }
            };
        }

        if (!resolution.Success)
        {
            await AuditAsync(
                context,
                "UNKNOWN",
                "TOOL_RESOLUTION_FAILED",
                reason:
                    $"Unable to confidently resolve tool. Score={resolution.Score}");

            return Error(
                "Unable to confidently determine requested action.");
        }

        var tool = resolution.Tool!;

        if (tool == null)
        {
            return Error("Sorry, I didn't understand your request.");
        }

        var compatible = _compatibility.IsCompatible(tool, context.RequestedToolVersion);

        if (!compatible)
        {
            await AuditAsync(
                context,
                tool.Name,
                "INCOMPATIBLE_TOOL_VERSION");

            return Error(
                "Requested tool version is not supported.");
        }

        var executionContext = BuildExecutionContext(
            request,
            tool);

        var validationResponse = await ValidateRequestAsync(
            executionContext,
            context);

        if (validationResponse != null)
        {
            return validationResponse;
        }

        var policyResponse = await EvaluatePolicyAsync(
            tool,
            executionContext,
            context);

        if (policyResponse != null)
        {
            return policyResponse;
        }

        var idempotencyResponse =
            await HandleIdempotencyAsync(
                request,
                context);

        if (idempotencyResponse != null)
        {
            return idempotencyResponse;
        }

        var rateLimitResult =
    await _rateLimit.CheckAsync(
        context,
        tool.Name,
        tool.Definition.Risk);

        if (!rateLimitResult.Allowed)
        {
            await AuditAsync(
                context,
                tool.Name,
                "RATE_LIMITED",
                reason: rateLimitResult.Reason);

            return Error(
                $"Rate limit exceeded. Retry after UTC {rateLimitResult.ResetAtUtc:u}");
        }

        var lockKey = _lockKeyBuilder.Build(tool, context, request);

        try
        {
            var acquired =
            await _lockService.AcquireAsync(
                lockKey,
                TimeSpan.FromSeconds(30));

            if (!acquired)
            {
                await AuditAsync(
                    context,
                    tool.Name,
                    "EXECUTION_LOCKED");

                return Error(
                    "Another similar operation is already in progress.");
            }
        }
        finally
        {
            await _lockService.ReleaseAsync(lockKey);
        }

        var response = await ExecuteToolAsync(
            tool,
            request,
            context);

        await SaveIdempotencyAsync(
            request,
            context,
            response);

        return response;

    }

    private ToolExecutionContext BuildExecutionContext(
        McpRequest request,
        IMcpTool tool)
    {
        return _extractor.Extract(
            request.Input,
            tool.Name);
    }

    // VALIDATION

    private async Task<McpResponse?> ValidateRequestAsync(
        ToolExecutionContext executionContext,
        RequestContext context)
    {
        var validationResult =
            _validator.Validate(executionContext);

        if (validationResult.Valid)
        {
            return null;
        }

        await AuditAsync(
            context,
            executionContext.ToolName,
            "VALIDATION_FAILED",
            executionContext.Parameters,
            string.Join(", ", validationResult.Errors));

        return Error(
            "Validation failed: " +
            string.Join(", ", validationResult.Errors));
    }

    // POLICY

    private async Task<McpResponse?> EvaluatePolicyAsync(
        IMcpTool tool,
        ToolExecutionContext executionContext,
        RequestContext context)
    {
        var policyResult =
            _policy.Evaluate(
                tool.Definition,
                context);

        if (!policyResult.Allowed)
        {
            await AuditAsync(
                context,
                tool.Name,
                "POLICY_DENIED",
                executionContext.Parameters,
                policyResult.Reason);

            return Error(
                $"Not allowed: {policyResult.Reason}");
        }

        if (policyResult.RequiresConfirmation)
        {
            await AuditAsync(
                context,
                tool.Name,
                "CONFIRMATION_REQUIRED",
                executionContext.Parameters);

            return BuildConfirmationResponse(
                tool,
                executionContext);
        }

        return null;
    }

    // CONFIRMATION FLOW

    private async Task<McpResponse> HandleConfirmationAsync(
        McpRequest request,
        RequestContext context)
    {
        if (request.PendingAction == null ||
            string.IsNullOrEmpty(
                request.PendingActionSignature))
        {
            return Error("Invalid confirmation payload");
        }

        var valid =
            _pendingActionService.Verify(
                request.PendingAction,
                request.PendingActionSignature);

        if (!valid)
        {
            await AuditAsync(
                context,
                request.PendingAction.ToolName,
                "INVALID_CONFIRMATION");

            return Error(
                "Invalid or expired confirmation");
        }

        var tool =
            _registry.GetByName(
                request.PendingAction.ToolName);

        return await ExecuteToolAsync(
            tool,
            request,
            context
            );
    }

    private McpResponse BuildConfirmationResponse(
        IMcpTool tool,
        ToolExecutionContext executionContext)
    {
        var pendingAction = new PendingAction
        {
            ToolName = tool.Name,
            Parameters = executionContext.Parameters,
            ExpiresAt = DateTimeOffset.UtcNow
                .AddMinutes(5)
                .ToUnixTimeSeconds()
        };

        var signature =
            _pendingActionService.Sign(
                pendingAction);

        return new McpResponse
        {
            Response =
                $"{tool.Definition.Name} requires confirmation",

            Actions =
            [
                new ActionItem
                {
                    Type = tool.Name,
                    RequiresConfirmation = true,
                    Payload = new
                    {
                        pendingAction,
                        signature
                    }
                }
            ]
        };
    }

    // IDEMPOTENCY

    private async Task<McpResponse?> HandleIdempotencyAsync(
        McpRequest request,
        RequestContext context)
    {
        if (string.IsNullOrEmpty(
                context.IdempotencyKey))
        {
            return null;
        }

        var requestHash =
            _hashService.Compute(
                request.Input);

        var existing =
            await _idempotency.GetAsync(
                context.MerchantId,
                context.IdempotencyKey);

        if (existing == null)
        {
            return null;
        }

        if (existing.RequestHash != requestHash)
        {
            return Error(
                "Idempotency key reused with different request");
        }

        return JsonConvert.DeserializeObject<McpResponse>(
            existing.ResponseJson);
    }

    private async Task SaveIdempotencyAsync(
        McpRequest request,
        RequestContext context,
        McpResponse response)
    {
        if (string.IsNullOrEmpty(
                context.IdempotencyKey))
        {
            return;
        }

        var requestHash =
            _hashService.Compute(
                request.Input);

        await _idempotency.SaveAsync(
            new IdempotencyRecord
            {
                Key = context.IdempotencyKey,
                MerchantId = context.MerchantId,
                RequestHash = requestHash,
                ResponseJson =
                    JsonConvert.SerializeObject(response),
                CreatedAtUtc = DateTime.UtcNow
            });
    }

    // EXECUTION

    private async Task<McpResponse> ExecuteToolAsync(
    IMcpTool tool,
    McpRequest request,
    RequestContext context)
    {
        var sw = Stopwatch.StartNew();
        var timeoutSeconds =
            tool.Definition.Risk == RiskLevel.High
                ? _executionSettings.HighRiskTimeoutSeconds
                : _executionSettings.DefaultTimeoutSeconds;

        using var cts =
            new CancellationTokenSource(
                TimeSpan.FromSeconds(timeoutSeconds));

        try
        {
            var result =
                await tool.ExecuteAsync(
                    request,
                    context,
                    cts.Token);

            await AuditAsync(
                context,
                tool.Name,
                "SUCCESS");

            sw.Stop();

            _metrics.RecordToolExecution(
                tool.Name,
                "SUCCESS",
                sw.ElapsedMilliseconds);

            return new McpResponse
            {
                Response = result.Message,
                Data = result.Data
            };
        }
        catch (OperationCanceledException)
        {
            await AuditAsync(
                context,
                tool.Name,
                "EXECUTION_TIMEOUT");

            return Error(
                "Tool execution timed out");
        }
        catch (BrokenCircuitException)
        {
            await AuditAsync(
                context,
                tool.Name,
                "CIRCUIT_OPEN");

            return Error(
                "Service temporarily unavailable");
        }
        catch (Exception ex)
        {
            await AuditAsync(
                context,
                tool.Name,
                "EXECUTION_FAILED",
                reason: ex.Message);

            return Error(
                "Internal execution error");
        }

    }

    // AUDIT

    private async Task AuditAsync(
        RequestContext context,
        string toolName,
        string status,
        Dictionary<string, ExtractedParameter>? parameters = null,
        string? reason = null)
    {
        await _audit.LogAsync(
            new AuditLogEntry
            {
                MerchantId = context.MerchantId,
                ClientId = context.ClientId,
                ToolName = toolName,
                Status = status,
                Parameters = parameters,
                Reason = reason,
                TimestampUtc = DateTime.UtcNow,
                CorrelationId = context.CorrelationId
            });
    }

    // RESPONSE HELPERS

    private static McpResponse Error(
        string message)
    {
        return new McpResponse
        {
            Response = message
        };
    }
}

[Serializable]
internal class BrokenCircuitException : Exception
{
    public BrokenCircuitException()
    {
    }

    public BrokenCircuitException(string? message) : base(message)
    {
    }

    public BrokenCircuitException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}