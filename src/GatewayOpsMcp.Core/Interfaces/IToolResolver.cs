using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;

public interface IToolResolver
{
    ToolResolutionResult Resolve(string input);
}