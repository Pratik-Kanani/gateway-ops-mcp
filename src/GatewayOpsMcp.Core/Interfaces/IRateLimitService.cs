using GatewayOpsMcp.Core.Enums;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;

public interface IRateLimitService
{
    Task<RateLimitResult> CheckAsync(
        RequestContext context,
        string toolName,
        RiskLevel risk);
}