namespace GatewayOpsMcp.Core.Models;

public class ToolDependency
{
    public string ToolName { get; set; }

    public bool Required { get; set; }

    public string Purpose { get; set; }

    public ToolDependency()
    {
        ToolName = string.Empty;

        Purpose = string.Empty;
    }
}