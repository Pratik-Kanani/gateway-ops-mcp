namespace GatewayOpsMcp.Core.Interfaces;

public interface IMetricsService
{
    void RecordToolExecution(
        string toolName,
        string status,
        double durationMs);

    void RecordRateLimit(
        string toolName);

    void RecordCircuitBreaker(
        string toolName);

    void RecordTimeout(
        string toolName);
}