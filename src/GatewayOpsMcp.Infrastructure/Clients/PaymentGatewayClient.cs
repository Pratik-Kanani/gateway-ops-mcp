using GatewayOpsMcp.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace GatewayOpsMcp.Infrastructure.Clients;
public class PaymentGatewayClient : IPaymentGatewayClient
{
    private readonly IMerchantService _merchantService;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    public PaymentGatewayClient(
        IMerchantService merchantService,
        IHttpClientFactory clientFactory,
        IConfiguration configuration)
    {
        _merchantService = merchantService;
        _clientFactory = clientFactory;
        _configuration = configuration;
    }

    public async Task<string> GetTransaction( 
        string merchantId, 
        string txnId, 
        CancellationToken cancellationToken)
    {
        var baseAddress = _configuration["PaymentGateway:ApiBaseAddress"]!;
        var creds = await _merchantService.GetCredentials(merchantId);

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{baseAddress}/{txnId}");

        request.Headers.Add("X-API-KEY", creds.AccessKey);
        request.Headers.Add("X-API-SECRET", creds.Secret);

        var _http = _clientFactory.CreateClient();

        var response = await _http.SendAsync(request, cancellationToken);

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task<string> CreatePaymentLink( 
        string merchantId, 
        double amount, 
        CancellationToken cancellationToken)
    {
        var baseAddress = _configuration["PaymentGateway:ApiBaseAddress"]!;
        var creds = await _merchantService.GetCredentials(merchantId);

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"{baseAddress}/{merchantId}");

        request.Headers.Add("X-API-KEY", creds.AccessKey);
        request.Headers.Add("X-API-SECRET", creds.Secret);

        var _http = _clientFactory.CreateClient();

        var response = await _http.SendAsync(request, cancellationToken);

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}