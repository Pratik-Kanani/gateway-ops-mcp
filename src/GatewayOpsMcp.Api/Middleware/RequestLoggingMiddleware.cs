using System.Diagnostics;

namespace GatewayOpsMcp.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        var correlationId =
            context.Items["CorrelationId"]?.ToString();

        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();

            _logger.LogInformation(
                "HTTP {Method} {Path} {StatusCode} {DurationMs}ms CorrelationId={CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds,
                correlationId);
        }
    }
}