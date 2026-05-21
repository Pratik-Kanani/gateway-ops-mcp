namespace GatewayOpsMcp.Core.Models;

public class McpRequest
{
    public string Input { get; set; }
    public Dictionary<string, string>? Context { get; set; }

    public PendingAction? PendingAction { get; set; }

    public string? PendingActionSignature { get; set; }

    public bool Confirm { get; set; } = false;

    public McpRequest()
    {
        Input = string.Empty;
    }
}