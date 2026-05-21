namespace GatewayOpsMcp.Core.Models;
public class ActionItem
{
    public string Type { get; set; }
    public bool RequiresConfirmation { get; set; }
    public object? Payload { get; set; }

    public ActionItem()
    {
        Type = string.Empty;
    }
}