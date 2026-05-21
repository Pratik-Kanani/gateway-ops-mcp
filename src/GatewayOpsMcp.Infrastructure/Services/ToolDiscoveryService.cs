using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Infrastructure.Services;

public class ToolDiscoveryService
    : IToolDiscoveryService
{
    private readonly IToolRegistry _registry;

    public ToolDiscoveryService(
        IToolRegistry registry)
    {
        _registry = registry;
        
    }

    public IEnumerable<ToolDescriptor> GetTools()
    {
        return _registry
            .GetAll()
            .Select(tool => new ToolDescriptor
            {
                Name = tool.Name,
                Definition = tool.Definition,
                Schema = tool.Schema,
                Version = tool.Version,
                Capabilities = tool.Capabilities,
                Dependencies = tool.Dependencies,
                Stage = tool.Stage
            });
    }
}