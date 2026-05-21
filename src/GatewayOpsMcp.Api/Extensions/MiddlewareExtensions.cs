using GatewayOpsMcp.Api.Middleware;

namespace GatewayOpsMcp.Api.Extensions;

public static class MiddlewareExtensions
{
    public static WebApplication
        UseGatewayMiddleware(
            this WebApplication app)
    {
        app.UseMiddleware<
            CorrelationMiddleware>();

        app.UseMiddleware<
            RequestLoggingMiddleware>();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseMiddleware<
            ContextMiddleware>();

        app.UseMiddleware<HmacMiddleware>();

        return app;
    }
}