namespace GatewayOpsMcp.Core.Models;

public class ExecutionLockResult
{
    public bool Acquired { get; set; }

    public string? Reason { get; set; }
}