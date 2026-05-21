using System.Diagnostics;
using System.Diagnostics.Metrics;
using GatewayOpsMcp.Core.Interfaces;

namespace GatewayOpsMcp.Infrastructure.Observability;

public class MetricsService
    : IMetricsService
{
    private readonly Counter<long> _toolExecutions;

    private readonly Counter<long> _rateLimits;

    private readonly Counter<long> _circuitBreakers;

    private readonly Counter<long> _timeouts;

    private readonly Histogram<double> _executionLatency;

    public MetricsService()
    {
        var meter =
            new Meter(
                "GatewayOpsMcp",
                "1.0.0");

        _toolExecutions =
            meter.CreateCounter<long>(
                "tool_executions");

        _rateLimits =
            meter.CreateCounter<long>(
                "tool_rate_limits");

        _circuitBreakers =
            meter.CreateCounter<long>(
                "tool_circuit_breakers");

        _timeouts =
            meter.CreateCounter<long>(
                "tool_timeouts");

        _executionLatency =
            meter.CreateHistogram<double>(
                "tool_execution_latency_ms");
    }

    public void RecordToolExecution(
        string toolName,
        string status,
        double durationMs)
    {
        var tags = new TagList
        {
            { "tool", toolName },
            { "status", status }
        };

        _toolExecutions.Add(
            1,
            tags);

        _executionLatency.Record(
            durationMs,
            tags);
    }

    public void RecordRateLimit(
        string toolName)
    {
        _rateLimits.Add(
            1,
            new TagList
            {
                { "tool", toolName }
            });
    }

    public void RecordCircuitBreaker(
        string toolName)
    {
        _circuitBreakers.Add(
            1,
            new TagList
            {
                { "tool", toolName }
            });
    }

    public void RecordTimeout(
        string toolName)
    {
        _timeouts.Add(
            1,
            new TagList
            {
                { "tool", toolName }
            });
    }
}