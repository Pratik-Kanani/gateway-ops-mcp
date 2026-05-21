using GatewayOpsMcp.Core.Enums;
using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Tools.Implementations;

public class CreatePaymentLinkTool : IMcpTool
{
    private readonly IPaymentGatewayClient _client;

    public CreatePaymentLinkTool(IPaymentGatewayClient client)
    {
        _client = client;
    }

    public string Name => "CreatePaymentLink";

    public ActionDefinition Definition => new()
    {
        Name = "CreatePaymentLink",
        RequiredScope = "payments:write",
        IsWrite = true,
        Risk = RiskLevel.Medium
    };

    public async Task<ToolResult> ExecuteAsync(
        McpRequest request,
        RequestContext context,
        CancellationToken cancellationToken)
    {

        var amount = request.PendingAction?
         .Parameters?
         .GetValueOrDefault("amount")
         ?.Value;

        var paymentLink = await _client.CreatePaymentLink(context.MerchantId, (double)amount!, cancellationToken);

        return new ToolResult
        {
            Message = $"Payment link created for ₹{amount}",
            Data = new
            {
                paymentLink,
                amount
            }
        };
    }

    public ToolSchema Schema => new()
    {
        ToolName = Name,
        Parameters =
    [
        new ToolParameterDefinition
        {
            Name = "amount",
            Type = "number",
            Required = true,
            Min = 1,
            Max = 100000,
            Description = "Payment amount in INR"
        },

        new ToolParameterDefinition
        {
            Name = "orderId",
            Type = "string",
            Required = false,
            Description = "Merchant order identifier"
        }
    ]
    };

    public ToolMetadata Metadata => new()
    {
        Description =
        "Creates payment links for customers",

        Keywords =
    [
        "payment",
        "link",
        "collect",
        "upi",
        "invoice",
        "pay"
    ],

        Examples =
    [
        "Create payment link for ₹500",
        "Send payment request",
        "Generate UPI payment link"
    ],
        IntentPhrases =
[
    "request payment",
    "collect funds",
    "generate invoice"
]
    };
    public List<ToolCapability> Capabilities =>
    [
        new()
    {
        Name = "payment_links",
        Category = "payments"
    },

    new()
    {
        Name = "collections",
        Category = "payments"
    },

    new()
    {
        Name = "upi",
        Category = "payments"
    }
    ];
    public ToolCompatibility Compatibility => new()
    {
        MinimumSupportedMajor = 1,
        BackwardCompatible = true
    };
    public ToolVersion Version => new()
    {
        Major = 1,
        Minor = 0
    };
    public List<ToolDependency> Dependencies => [];
    public WorkflowStage Stage => WorkflowStage.Execution;
}