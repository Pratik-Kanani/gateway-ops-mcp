using System.Text;
using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;
using GatewayOpsMcp.Infrastructure.Security;

namespace GatewayOpsMcp.Api.Middleware;
public class HmacMiddleware
{
    private readonly RequestDelegate _next;

    public HmacMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
    HttpContext context,
    IHmacService hmacService,
    IMerchantService merchantService)
    {
        if (context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }


        if (context.Items["ctx"] is not RequestContext ctx || string.IsNullOrEmpty(ctx.MerchantId))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid context");
            return;
        }

        var merchant = await merchantService.GetCredentials(ctx.MerchantId);

        var signature = context.Request.Headers["X-Signature"].FirstOrDefault();
        var timestamp = context.Request.Headers["X-Timestamp"].FirstOrDefault();

        if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(timestamp))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Missing HMAC headers");
            return;
        }

        if (!long.TryParse(timestamp, out var ts))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid timestamp");
            return;
        }

        var requestTime = DateTimeOffset.FromUnixTimeSeconds(ts);

        if (DateTimeOffset.UtcNow - requestTime > TimeSpan.FromMinutes(5))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Request expired");
            return;
        }

        context.Request.EnableBuffering();

        string body;
        using (var reader = new StreamReader(
                   context.Request.Body,
                   Encoding.UTF8,
                   leaveOpen: true))
        {
            body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        var payload = $"{context.Request.Method}{context.Request.Path}{body}{timestamp}";

        var computed = hmacService.GenerateSignature(merchant.HmacSecret, payload);

        if (!computed.Equals(signature, StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid signature");
            return;
        }

        await _next(context);
    }
}