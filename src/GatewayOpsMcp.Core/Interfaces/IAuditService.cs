using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;

public interface IAuditService
{
    Task LogAsync(AuditLogEntry entry);
}