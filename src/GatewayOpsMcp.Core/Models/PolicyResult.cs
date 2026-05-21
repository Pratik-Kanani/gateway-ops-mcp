namespace GatewayOpsMcp.Core.Models;
public class PolicyResult
{
    public bool Allowed { get; set; }
    public bool RequiresConfirmation { get; set; }
    public string Reason { get; set; }

    public PolicyResult()
    {
        Reason = string.Empty;
    }
}