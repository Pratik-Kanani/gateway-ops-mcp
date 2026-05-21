namespace GatewayOpsMcp.Core.Interfaces;

public interface IRequestHashService
{
    string Compute(string input);
}