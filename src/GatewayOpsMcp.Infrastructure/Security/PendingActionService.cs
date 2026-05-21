using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;
using Newtonsoft.Json;

namespace GatewayOpsMcp.Infrastructure.Security;
public class PendingActionService : IPendingActionService
{
    private readonly IHmacService _hmac;
    private const string Secret = "pending-action-secret";

    public PendingActionService(IHmacService hmac)
    {
        _hmac = hmac;
    }

    public string Sign(PendingAction action)
    {
        var payload = BuildPayload(action);

        return _hmac.GenerateSignature(Secret, payload);
    }

    public bool Verify(PendingAction action, string signature)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        if (now > action.ExpiresAt)
            return false;

        var computed = Sign(action);

        return computed.Equals(signature, StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildPayload(PendingAction action)
    {
        return JsonConvert.SerializeObject(action);
    }
}