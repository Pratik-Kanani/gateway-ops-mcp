using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Tools.Implementations;

namespace GatewayOpsMcp.Infrastructure.Services;

public class ToolRegistry : IToolRegistry
{
    private readonly List<IMcpTool> _tools;

    public ToolRegistry(
        DiagnosePaymentIssueTool diagnose,
        CreatePaymentLinkTool createLink)
    {
        _tools =
        [
            diagnose,
            createLink
        ];
    }

    public IMcpTool? Resolve(string input)
    {
        if (input.Contains("fail", StringComparison.OrdinalIgnoreCase))
            return _tools.First(x => x.Name == "DiagnosePaymentIssue");

        if (input.Contains("link", StringComparison.OrdinalIgnoreCase) ||
            input.Contains("pay", StringComparison.OrdinalIgnoreCase))
            return _tools.First(x => x.Name == "CreatePaymentLink");

        return null;
    }

    public IMcpTool GetByName(string name)
    {
        return _tools.First(x =>
            x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public IEnumerable<IMcpTool> GetAll()
    {
        return _tools;
    }
}