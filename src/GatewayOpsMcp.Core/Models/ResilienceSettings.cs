namespace GatewayOpsMcp.Core.Models;

public class ResilienceSettings
{
    public int RetryCount { get; set; } = 3;

    public int CircuitBreakerFailures { get; set; } = 5;

    public int CircuitBreakerDurationSeconds { get; set; } = 30;
}