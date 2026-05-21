using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;

public interface IToolDiscoveryService
{
    IEnumerable<ToolDescriptor> GetTools();
}