namespace GatewayOpsMcp.Core.Models;

public class ToolSchema
{
    public string ToolName { get; set; }

    public List<ToolParameterDefinition> Parameters { get; set; }

    public ToolSchema()
    {
        ToolName = string.Empty;
        Parameters = [];
    }
}