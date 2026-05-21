using System.Security.Claims;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Api.Middleware;
public class ContextMiddleware
{
    private readonly RequestDelegate _next;

    public ContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var merchantId = context.User.FindFirst("sub")?.Value ??
                         context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var clientId = context.User.FindFirst("client_id")?.Value;
        var scope = context.User.FindFirst("scope")?.Value;

        var idempotencyKey = context.Request.Headers["X-Idempotency-Key"].FirstOrDefault();

        var correlationId = context.Items["CorrelationId"]?.ToString();

        var toolVersion = context.Request.Headers["X-Tool-Version"].FirstOrDefault();

        var scopes = scope?.Split(' ') ?? [];

        if(string.IsNullOrEmpty(merchantId))
        {
            throw new Exception("Invalid Merchant Id");
        }

        if(string.IsNullOrEmpty(clientId))
        {
            throw new Exception("Invalid Client Id");
        }

        context.Items["ctx"] = new RequestContext
        {
            MerchantId = merchantId,
            ClientId = clientId,
            Scopes = scopes,
            IdempotencyKey = idempotencyKey,
            CorrelationId = correlationId ?? Guid.NewGuid().ToString(),
            RequestedToolVersion = toolVersion
        };

        await _next(context);
    }
}