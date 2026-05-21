using GatewayOpsMcp.Core.Interfaces;

namespace GatewayOpsMcp.Core.Models;

public class ToolResolutionResult
{
    public IMcpTool? Tool { get; set; }

    public int Score { get; set; }

    public bool IsAmbiguous { get; set; }

    public List<string> CandidateTools { get; set; }

    public bool Success =>
        Tool != null && !IsAmbiguous;

    public ToolResolutionResult()
    {
        CandidateTools = [];
    }
}