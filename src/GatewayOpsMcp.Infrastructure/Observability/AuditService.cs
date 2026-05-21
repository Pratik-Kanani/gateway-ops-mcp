using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GatewayOpsMcp.Infrastructure.Observability;

public class AuditService : IAuditService
{
    private readonly ILogger<AuditService> _logger;

    public AuditService(
        ILogger<AuditService> logger)
    {
        _logger = logger;
    }

    public Task LogAsync(AuditLogEntry entry)
    {
        var json = JsonConvert.SerializeObject(entry);

        _logger.LogInformation(
            "AUDIT_LOG: {AuditEntry}",
            json);

        return Task.CompletedTask;
    }
}