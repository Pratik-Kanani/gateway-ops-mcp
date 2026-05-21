using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;

public interface IParameterExtractionService
{
    ToolExecutionContext Extract(
        string input,
        string toolName);
}