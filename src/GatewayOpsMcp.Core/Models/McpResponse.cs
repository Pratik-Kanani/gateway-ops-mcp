namespace GatewayOpsMcp.Core.Models;
public class McpResponse
{
    public string Response { get; set; }
    public object? Data { get; set; }
    public List<ActionItem> Actions { get; set; } = [];

    public McpResponse()
    {
        Response = string.Empty;
    }
}