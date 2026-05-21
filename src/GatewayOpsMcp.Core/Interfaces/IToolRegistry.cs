namespace GatewayOpsMcp.Core.Interfaces;
public interface IToolRegistry
{
    IMcpTool? Resolve(string input);

    IMcpTool GetByName(string name);
    IEnumerable<IMcpTool> GetAll();
}