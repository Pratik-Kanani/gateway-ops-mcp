using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;

public interface IIdempotencyService
{
    Task<IdempotencyRecord?> GetAsync(
        string merchantId,
        string key);

    Task SaveAsync(
        IdempotencyRecord record);
}