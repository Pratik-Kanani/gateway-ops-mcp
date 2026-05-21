namespace GatewayOpsMcp.Api.Middleware;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId =
            context.Request.Headers["X-Correlation-Id"]
                .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        // store for request lifecycle
        context.Items["CorrelationId"] =
            correlationId;

        // return to caller
        context.Response.Headers["X-Correlation-Id"] =
            correlationId;

        await _next(context);
    }
}