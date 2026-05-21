using Dapper;
using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace GatewayOpsMcp.Infrastructure.Services;
public class MerchantService : IMerchantService
{
    private readonly string _connectionString;
    private readonly IMemoryCache _cache;

    public MerchantService(
        IConfiguration config, 
        IMemoryCache cache)
    {
        _connectionString = config.GetConnectionString("Default")!;
        _cache = cache;
    }

    public async Task<MerchantCreds> GetCredentials(string merchantId)
    {
        if (_cache.TryGetValue(merchantId, out MerchantCreds? cached) && cached is not null)
            return cached;

        using var conn = new MySqlConnection(_connectionString);

        var creds = await conn.QueryFirstOrDefaultAsync<MerchantCreds>(
            @"SELECT merchant_id AS MerchantId,
                     access_key AS AccessKey,
                     secret AS Secret,
                     hmac_secret AS HmacSecret
              FROM merchants
              WHERE merchant_id = @id",
            new { id = merchantId }) ?? throw new Exception("Merchant not found");
        _cache.Set(merchantId, creds, TimeSpan.FromMinutes(10));

        return creds;
    }
}