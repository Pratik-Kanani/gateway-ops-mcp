using OpenTelemetry.Metrics;

namespace GatewayOpsMcp.Api.Extensions;

public static class ObservabilityExtensions
{
    public static IServiceCollection
        AddGatewayObservability(
            this IServiceCollection services)
    {
        services
            .AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddMeter("GatewayOpsMcp")
                    .AddConsoleExporter();
            });

        return services;
    }
}