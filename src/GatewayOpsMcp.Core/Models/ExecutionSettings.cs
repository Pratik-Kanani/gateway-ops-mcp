namespace GatewayOpsMcp.Core.Models;

public class ExecutionSettings
{
    public int DefaultTimeoutSeconds { get; set; } = 15;

    public int HighRiskTimeoutSeconds { get; set; } = 10;
}