namespace GatewayOpsMcp.Core.Models;
public class MerchantCreds
{
    public string MerchantId { get; set; }
    public string AccessKey { get; set; }
    public string Secret { get; set; }
    public string HmacSecret { get; set; }

    public MerchantCreds()
    {
        MerchantId = string.Empty;
        AccessKey = string.Empty;
        Secret = string.Empty;
        HmacSecret = string.Empty;
    }
}