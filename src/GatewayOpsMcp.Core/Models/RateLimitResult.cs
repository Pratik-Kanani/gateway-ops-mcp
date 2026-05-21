namespace GatewayOpsMcp.Core.Models;

public class RateLimitResult
{
    public bool Allowed { get; set; }

    public int Remaining { get; set; }

    public DateTime ResetAtUtc { get; set; }

    public string? Reason { get; set; }
}