using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;

public interface IExecutionLockKeyBuilder
{
    string Build(
        IMcpTool tool,
        RequestContext context,
        McpRequest request);
}