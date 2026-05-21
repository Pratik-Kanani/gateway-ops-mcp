namespace GatewayOpsMcp.Core.Models;
public class PendingAction
{
    public string ToolName { get; set; }
    public Dictionary<string, ExtractedParameter>? Parameters { get; set; }
    public long ExpiresAt { get; set; }

    public PendingAction()
    {
        ToolName = string.Empty;
    }
}