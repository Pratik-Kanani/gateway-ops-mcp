using System.Security.Cryptography;
using System.Text;
using GatewayOpsMcp.Core.Interfaces;

namespace GatewayOpsMcp.Infrastructure.Security;

public class RequestHashService
    : IRequestHashService
{
    public string Compute(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);

        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }
}