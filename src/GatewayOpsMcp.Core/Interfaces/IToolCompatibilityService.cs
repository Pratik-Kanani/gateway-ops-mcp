namespace GatewayOpsMcp.Core.Interfaces;

public interface IToolCompatibilityService
{
    bool IsCompatible(
        IMcpTool tool,
        string? requestedVersion);
}