namespace GatewayOpsMcp.Core.Models;

public class ToolExecutionContext
{
    public string ToolName { get; set; }

    public Dictionary<string, ExtractedParameter> Parameters { get; set; }

    public ToolExecutionContext()
    {
        ToolName = string.Empty;
        Parameters = [];
    }
}