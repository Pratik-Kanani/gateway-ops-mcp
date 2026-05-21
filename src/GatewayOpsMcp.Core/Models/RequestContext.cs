namespace GatewayOpsMcp.Core.Models;
public class RequestContext
{
    public string MerchantId { get; set; }
    public string ClientId { get; set; }
    public string[] Scopes { get; set; }
    public string? IdempotencyKey { get; set; }
    public string CorrelationId { get; set; }
    public string? RequestedToolVersion { get; set; }

    public RequestContext()
    {
        MerchantId = string.Empty;
        ClientId = string.Empty;
        Scopes = [];
        CorrelationId = string.Empty;
    }
}