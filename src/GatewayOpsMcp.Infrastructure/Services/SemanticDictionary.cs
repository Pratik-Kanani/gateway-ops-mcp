using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Infrastructure.Services;

public class SemanticDictionary
    : ISemanticDictionary
{
    private readonly List<SemanticEntity> _entities =
    [
        new()
        {
            Name = "payment_link",

            CanonicalValue =
                "CreatePaymentLink",

            Synonyms =
            [
                "payment link",
                "collect money",
                "request payment",
                "invoice",
                "upi request",
                "payment request",
                "collect payment"
            ]
        },

        new()
        {
            Name = "diagnostics",

            CanonicalValue =
                "DiagnosePaymentIssue",

            Synonyms =
            [
                "payment failed",
                "transaction failed",
                "diagnose payment",
                "payment issue",
                "check transaction",
                "txn failure"
            ]
        }
    ];

    public IEnumerable<SemanticEntity>
        GetEntities()
    {
        return _entities;
    }
}