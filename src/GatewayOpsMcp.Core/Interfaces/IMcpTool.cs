using GatewayOpsMcp.Core.Enums;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;
public interface IMcpTool
{
    string Name { get; }

    ToolSchema Schema { get; }

    ActionDefinition Definition { get; }
    
    ToolMetadata Metadata { get; }

    ToolVersion Version { get; }

    ToolCompatibility Compatibility { get; }

    List<ToolCapability> Capabilities { get; }
    List<ToolDependency> Dependencies { get; }
    WorkflowStage Stage { get; }

    Task<ToolResult> ExecuteAsync(
        McpRequest request, 
        RequestContext context,
        CancellationToken cancellationToken);
}