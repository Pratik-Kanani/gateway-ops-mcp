using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;
public interface IMerchantService
{
    Task<MerchantCreds> GetCredentials(string merchantId);
}