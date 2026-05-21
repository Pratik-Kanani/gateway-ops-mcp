using GatewayOpsMcp.Core.Interfaces;

namespace GatewayOpsMcp.Infrastructure.Resilience;

public class ToolCompatibilityService
    : IToolCompatibilityService
{
    public bool IsCompatible(
        IMcpTool tool,
        string? requestedVersion)
    {
        if (string.IsNullOrWhiteSpace(
                requestedVersion))
        {
            return true;
        }

        var parts =
            requestedVersion.Split('.');

        if (parts.Length != 2)
        {
            return false;
        }

        if (!int.TryParse(parts[0], out var major))
        {
            return false;
        }

        return major >=
            tool.Compatibility
                .MinimumSupportedMajor;
    }
}