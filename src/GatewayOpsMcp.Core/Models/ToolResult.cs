namespace GatewayOpsMcp.Core.Models;
public class ToolResult
{
    public string Message { get; set; }
    public object? Data { get; set; }

    public ToolResult()
    {
        Message = string.Empty;
    }
}