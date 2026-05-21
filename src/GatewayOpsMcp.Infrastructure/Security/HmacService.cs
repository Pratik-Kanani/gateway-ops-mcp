using System.Security.Cryptography;
using System.Text;

namespace GatewayOpsMcp.Infrastructure.Security;
public interface IHmacService
{
    string GenerateSignature(string secret, string payload);
}

public class HmacService : IHmacService
{
    public string GenerateSignature(string secret, string payload)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);

        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(payloadBytes);

        return Convert.ToHexString(hash).ToLower();
    }
}