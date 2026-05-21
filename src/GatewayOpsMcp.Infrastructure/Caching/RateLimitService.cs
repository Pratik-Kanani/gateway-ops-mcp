using GatewayOpsMcp.Core.Enums;
using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GatewayOpsMcp.Infrastructure.Caching;

public class RateLimitService
    : IRateLimitService
{
    private readonly IMemoryCache _cache;

    public RateLimitService(
        IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<RateLimitResult> CheckAsync(
        RequestContext context,
        string toolName,
        RiskLevel risk)
    {
        var limit = GetLimit(risk);

        var key =
            $"{context.MerchantId}:{toolName}";

        var now = DateTime.UtcNow;

        var window =
            TimeSpan.FromMinutes(1);

        var counter =
            _cache.GetOrCreate(
                key,
                entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow =
                        window;

                    return new RateLimitCounter
                    {
                        Count = 0,
                        ResetAtUtc = now.Add(window)
                    };
                });

        counter!.Count++;

        var allowed =
            counter.Count <= limit;

        return Task.FromResult(
            new RateLimitResult
            {
                Allowed = allowed,
                Remaining =
                    Math.Max(0, limit - counter.Count),

                ResetAtUtc = counter.ResetAtUtc,

                Reason = allowed
                    ? null
                    : "Rate limit exceeded"
            });
    }

    private static int GetLimit(
        RiskLevel risk)
    {
        return risk switch
        {
            RiskLevel.Low => 100,
            RiskLevel.Medium => 30,
            RiskLevel.High => 10,
            _ => 20
        };
    }

    private class RateLimitCounter
    {
        public int Count { get; set; }

        public DateTime ResetAtUtc { get; set; }
    }
}