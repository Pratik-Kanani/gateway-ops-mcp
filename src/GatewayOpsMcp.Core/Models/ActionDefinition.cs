using GatewayOpsMcp.Core.Enums;

namespace GatewayOpsMcp.Core.Models;

public class ActionDefinition
{
    public string Name { get; set; }
    public string RequiredScope { get; set; }
    public bool IsWrite { get; set; }
    public RiskLevel Risk { get; set; }

    public ActionDefinition()
    {
        Name = string.Empty;
        RequiredScope = string.Empty;
    }
}