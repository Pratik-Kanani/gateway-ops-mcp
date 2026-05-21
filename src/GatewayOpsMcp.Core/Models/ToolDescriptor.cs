using GatewayOpsMcp.Core.Enums;

namespace GatewayOpsMcp.Core.Models;

public class ToolDescriptor
{
    public string Name { get; set; }

    public ActionDefinition Definition { get; set; }

    public ToolSchema Schema { get; set; }

    public ToolVersion Version { get; set; }

    public List<ToolCapability> Capabilities { get; set; }
    public List<ToolDependency> Dependencies { get; set; }
    public WorkflowStage Stage { get; set; }

    public ToolDescriptor()
    {
        Name = string.Empty;

        Definition = new ActionDefinition();

        Schema = new ToolSchema();

        Version = new ToolVersion();
        
        Capabilities = [];
        Dependencies = [];
    }
}