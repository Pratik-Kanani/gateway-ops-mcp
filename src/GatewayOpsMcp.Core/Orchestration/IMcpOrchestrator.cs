using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Orchestration;
public interface IMcpOrchestrator
{
    Task<McpResponse> HandleAsync(
        McpRequest request, 
        RequestContext context);
}