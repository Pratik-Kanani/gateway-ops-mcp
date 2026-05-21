using GatewayOpsMcp.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace GatewayOpsMcp.Infrastructure.Caching;

public class ExecutionLockService
    : IExecutionLockService
{
    private readonly IMemoryCache _cache;

    public ExecutionLockService(
        IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<bool> AcquireAsync(
        string key,
        TimeSpan expiry)
    {
        if (_cache.TryGetValue(key, out _))
        {
            return Task.FromResult(false);
        }

        _cache.Set(
            key,
            true,
            expiry);

        return Task.FromResult(true);
    }

    public Task ReleaseAsync(
        string key)
    {
        _cache.Remove(key);

        return Task.CompletedTask;
    }
}