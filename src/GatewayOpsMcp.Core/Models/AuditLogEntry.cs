namespace GatewayOpsMcp.Core.Models;

public class AuditLogEntry
{
    public string MerchantId { get; set; }

    public string ClientId { get; set; }

    public string ToolName { get; set; }

    public string Status { get; set; }

    public Dictionary<string, ExtractedParameter>? Parameters { get; set; }

    public string? Reason { get; set; }

    public DateTime TimestampUtc { get; set; }
    public string CorrelationId { get; set; }

    public AuditLogEntry()
    {
        MerchantId = string.Empty;
        ClientId = string.Empty;
        ToolName = string.Empty;
        Status = string.Empty;
        CorrelationId = string.Empty;
    }
}