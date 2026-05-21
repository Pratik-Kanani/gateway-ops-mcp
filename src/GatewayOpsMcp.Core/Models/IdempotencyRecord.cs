namespace GatewayOpsMcp.Core.Models;

public class IdempotencyRecord
{
    public string Key { get; set; }

    public string MerchantId { get; set; }

    public string RequestHash { get; set; }

    public string ResponseJson { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public IdempotencyRecord()
    {
        Key = string.Empty;
        MerchantId = string.Empty;
        RequestHash = string.Empty;
        ResponseJson = string.Empty;
    }
}