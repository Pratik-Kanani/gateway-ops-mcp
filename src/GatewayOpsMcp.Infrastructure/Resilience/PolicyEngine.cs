using GatewayOpsMcp.Core.Enums;
using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Infrastructure.Resilience;
public class PolicyEngine : IPolicyEngine
{
    public PolicyResult Evaluate(ActionDefinition action, RequestContext context)
    {
        // 1. Scope validation
        if (!context.Scopes.Contains(action.RequiredScope))
        {
            return new PolicyResult
            {
                Allowed = false,
                Reason = $"Missing required scope: {action.RequiredScope}"
            };
        }

        // 2. Write actions need confirmation
        if (action.IsWrite)
        {
            return new PolicyResult
            {
                Allowed = true,
                RequiresConfirmation = true,
                Reason = "Write action requires confirmation"
            };
        }

        // 3. High-risk actions
        if (action.Risk == RiskLevel.High)
        {
            return new PolicyResult
            {
                Allowed = true,
                RequiresConfirmation = true,
                Reason = "High-risk action"
            };
        }

        // 4. Safe
        return new PolicyResult
        {
            Allowed = true,
            RequiresConfirmation = false
        };
    }
}