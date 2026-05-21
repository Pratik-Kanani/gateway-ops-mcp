namespace GatewayOpsMcp.Core.Models;

public class ToolParameterDefinition
{
    public string Name { get; set; }

    public string Type { get; set; }

    public bool Required { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public string? Description { get; set; }

    public ToolParameterDefinition()
    {
        Name = string.Empty;
        Type = string.Empty;
    }
}