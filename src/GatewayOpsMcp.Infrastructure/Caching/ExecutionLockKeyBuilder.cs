using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Infrastructure.Caching;

public class ExecutionLockKeyBuilder
    : IExecutionLockKeyBuilder
{
    public string Build(
        IMcpTool tool,
        RequestContext context,
        McpRequest request)
    {
        var entity =
            request.PendingAction?
                .Parameters?
                .FirstOrDefault()
                .Value?
                .Value?
                .ToString()
            ?? "global";

        return
            $"{context.MerchantId}:" +
            $"{tool.Name}:" +
            $"{entity}";
    }
}