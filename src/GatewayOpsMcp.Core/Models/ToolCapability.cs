namespace GatewayOpsMcp.Core.Models;

public class ToolCapability
{
    public string Name { get; set; }

    public string Category { get; set; }

    public ToolCapability()
    {
        Name = string.Empty;

        Category = string.Empty;
    }
}