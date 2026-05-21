using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GatewayOpsMcp.Infrastructure.Caching;

public class IdempotencyService
    : IIdempotencyService
{
    private readonly IMemoryCache _cache;

    public IdempotencyService(
        IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<IdempotencyRecord?> GetAsync(
        string merchantId,
        string key)
    {
        var cacheKey = BuildKey(
            merchantId,
            key);

        _cache.TryGetValue(
            cacheKey,
            out IdempotencyRecord? record);

        return Task.FromResult(record);
    }

    public Task SaveAsync(
        IdempotencyRecord record)
    {
        var cacheKey = BuildKey(
            record.MerchantId,
            record.Key);

        _cache.Set(
            cacheKey,
            record,
            TimeSpan.FromHours(24));

        return Task.CompletedTask;
    }

    private static string BuildKey(
        string merchantId,
        string key)
    {
        return $"{merchantId}:{key}";
    }
}