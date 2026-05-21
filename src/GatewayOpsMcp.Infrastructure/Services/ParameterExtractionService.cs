using System.Text.RegularExpressions;
using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Infrastructure.Services;

public partial class ParameterExtractionService
    : IParameterExtractionService
{
    public ToolExecutionContext Extract(
        string input,
        string toolName)
    {
        var ctx = new ToolExecutionContext
        {
            ToolName = toolName
        };

        switch (toolName)
        {
            case "CreatePaymentLink":
                ExtractPaymentLinkParams(input, ctx);
                break;

            case "DiagnosePaymentIssue":
                ExtractTransactionParams(input, ctx);
                break;
        }

        return ctx;
    }

    private static void ExtractPaymentLinkParams(
        string input,
        ToolExecutionContext ctx)
    {
        // ₹500 OR Rs 500
        var amountMatch = PaymentAmountPattern().Match(input);

        if (amountMatch.Success)
        {
            ctx.Parameters["amount"] =
                new ExtractedParameter
                {
                    Value =
                        int.Parse(
                            amountMatch.Groups[1].Value),

                    Confidence = 0.95,

                    Source = "regex",

                    RawValue = amountMatch.Value
                };
        }

        // order ORD123
        var orderMatch = OrderIdPattern().Match(input);

        if (orderMatch.Success)
        {
            ctx.Parameters["orderId"] =
                new ExtractedParameter
                {
                    Value =
                        orderMatch.Groups[1].Value,

                    Confidence = 0.90,

                    Source = "regex",

                    RawValue = orderMatch.Value
                };
        }
    }

    private static void ExtractTransactionParams(
        string input,
        ToolExecutionContext ctx)
    {
        var txnMatch = TransactionIdPattern().Match(input);

        if (txnMatch.Success)
        {
            ctx.Parameters["transactionId"] =
                new ExtractedParameter
                {
                    Value =
                        txnMatch.Groups[1].Value,

                    Confidence = 0.92,

                    Source = "regex",

                    RawValue = txnMatch.Value
                };
        }
    }

    [GeneratedRegex(@"(?:₹|rs\.?\s?)(\d+)", RegexOptions.IgnoreCase, "en-IN")]
    private static partial Regex PaymentAmountPattern();


    [GeneratedRegex(@"order\s+([A-Za-z0-9_-]+)", RegexOptions.IgnoreCase, "en-IN")]
    private static partial Regex OrderIdPattern();


    [GeneratedRegex(@"(?:txn|transaction)\s+([A-Za-z0-9_-]+)", RegexOptions.IgnoreCase, "en-IN")]
    private static partial Regex TransactionIdPattern();
}