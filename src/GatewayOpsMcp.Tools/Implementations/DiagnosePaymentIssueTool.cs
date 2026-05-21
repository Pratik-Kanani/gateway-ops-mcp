using GatewayOpsMcp.Core.Enums;
using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Tools.Implementations;

public class DiagnosePaymentIssueTool : IMcpTool
{
    private readonly IPaymentGatewayClient _client;
    public DiagnosePaymentIssueTool(IPaymentGatewayClient client)
    {
        _client = client;
    }

    public string Name => "DiagnosePaymentIssue";

    public ActionDefinition Definition => new()
    {
        Name = Name,
        RequiredScope = "payments:read",
        IsWrite = false,
        Risk = RiskLevel.Low
    };

    public async Task<ToolResult> ExecuteAsync(
        McpRequest request,
        RequestContext context,
        CancellationToken cancellationToken)
    {
        var txnId = "sample_txn"; // extract later

        var txn = await _client.GetTransaction(context.MerchantId, txnId, cancellationToken);

        return new ToolResult
        {
            Message = $"Transaction analysis for {txnId}",
            Data = txn
        };
    }

    public ToolSchema Schema => new()
    {
        ToolName = Name,
        Parameters =
    [
        new ToolParameterDefinition
        {
            Name = "transactionId",
            Type = "string",
            Required = true,
            Description = "Transaction identifier"
        }
    ]
    };

    public ToolMetadata Metadata => new()
    {
        Description =
        "Diagnoses failed or problematic transactions",

        Keywords =
    [
        "failed",
        "failure",
        "transaction",
        "payment issue",
        "declined",
        "timeout"
    ],

        Examples =
    [
        "Why did my payment fail?",
        "Check transaction status",
        "Diagnose txn PAY123"
    ],
        IntentPhrases =
[
    "success transaction",
    "diagnose payment",
    "failed transaction"
]
    };

    public List<ToolCapability> Capabilities =>
[
    new()
    {
        Name = "transaction_diagnostics",
        Category = "support"
    },

    new()
    {
        Name = "payment_failures",
        Category = "support"
    },

    new()
    {
        Name = "transaction_lookup",
        Category = "transactions"
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

    public WorkflowStage Stage => WorkflowStage.Retrieval;
}