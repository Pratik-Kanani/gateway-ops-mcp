using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;
public interface IPolicyEngine
{
    PolicyResult Evaluate(
        ActionDefinition action, 
        RequestContext context);
}