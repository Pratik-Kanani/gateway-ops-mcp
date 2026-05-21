using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;
using GatewayOpsMcp.Core.Orchestration;
using GatewayOpsMcp.Infrastructure.Caching;
using GatewayOpsMcp.Infrastructure.Clients;
using GatewayOpsMcp.Infrastructure.Observability;
using GatewayOpsMcp.Infrastructure.Orchestration;
using GatewayOpsMcp.Infrastructure.Resilience;
using GatewayOpsMcp.Infrastructure.Security;
using GatewayOpsMcp.Infrastructure.Services;
using Polly;

namespace GatewayOpsMcp.Api.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection
        AddGatewayInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        services.AddMemoryCache();

        // --------------------------------------------------
        // HTTP CLIENTS
        // --------------------------------------------------

        services
            .AddHttpClient<
                IPaymentGatewayClient,
                PaymentGatewayClient>()
            .AddTransientHttpErrorPolicy(
                policy =>
                    policy.WaitAndRetryAsync(
                        3,
                        retry =>
                            TimeSpan.FromMilliseconds(
                                200 * retry)))
            .AddTransientHttpErrorPolicy(
                policy =>
                    policy.CircuitBreakerAsync(
                        handledEventsAllowedBeforeBreaking: 5,
                        durationOfBreak:
                            TimeSpan.FromSeconds(30)));

        // --------------------------------------------------
        // SERVICES
        // --------------------------------------------------

        services.AddSingleton<
            IMerchantService,
            MerchantService>();

        services.AddSingleton<
            IParameterExtractionService,
            ParameterExtractionService>();

        services.AddSingleton<
            IValidationService,
            ValidationService>();

        services.AddSingleton<
            IPolicyEngine,
            PolicyEngine>();

        services.AddSingleton<
            IToolResolver,
            ToolResolver>();

        services.AddSingleton<
            IToolDiscoveryService,
            ToolDiscoveryService>();
        
        services.AddSingleton<
            IToolCompatibilityService,
            ToolCompatibilityService>();

        services.AddSingleton<
            ISemanticDictionary,
            SemanticDictionary>();
        
        services.AddSingleton<
            IMcpOrchestrator,
            McpOrchestrator>();

        // --------------------------------------------------
        // SECURITY
        // --------------------------------------------------

        services.AddSingleton<
            IHmacService,
            HmacService>();

        services.AddSingleton<
            IRequestHashService,
            RequestHashService>();

        services.AddSingleton<
            IPendingActionService,
            PendingActionService>();

        // --------------------------------------------------
        // CACHING / LOCKS
        // --------------------------------------------------

        services.AddSingleton<
            IIdempotencyService,
            IdempotencyService>();

        services.AddSingleton<
            IExecutionLockService,
            ExecutionLockService>();

        services.AddSingleton<
            IExecutionLockKeyBuilder,
            ExecutionLockKeyBuilder>();

        services.AddSingleton<
            IRateLimitService,
            RateLimitService>();

        // --------------------------------------------------
        // OBSERVABILITY
        // --------------------------------------------------

        services.AddSingleton<
            IMetricsService,
            MetricsService>();

        services.AddSingleton<
            IAuditService,
            AuditService>();

        // --------------------------------------------------

        services.Configure<ExecutionSettings>(
            configuration.GetSection("Execution"));

        services.Configure<ResilienceSettings>(
            configuration.GetSection("Resilience"));

        return services;
    }
}