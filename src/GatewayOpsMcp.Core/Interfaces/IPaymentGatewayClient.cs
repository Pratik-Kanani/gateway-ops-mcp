namespace GatewayOpsMcp.Core.Interfaces;
public interface IPaymentGatewayClient
{
    Task<string> GetTransaction(
        string merchantId,
        string txnId,
        CancellationToken cancellationToken);
    
    Task<string> CreatePaymentLink(
        string merchantId, 
        double amount,
        CancellationToken cancellationToken);
}