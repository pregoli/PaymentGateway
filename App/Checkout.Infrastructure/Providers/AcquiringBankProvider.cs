using Microsoft.Extensions.Logging;
using Checkout.Command.Application.Common.Dto;
using System.Collections.Generic;
using System.Linq;
using Checkout.Command.Application.Interfaces;

namespace Checkout.Infrastructure.Providers;

public class AcquiringBankProvider : IAcquiringBankProvider
{
    private readonly ILogger<AcquiringBankProvider> _logger;

    public AcquiringBankProvider(ILogger<AcquiringBankProvider> logger)
    {
        _logger = logger;
    }

    public TransactionAuthorizationResponse ValidateTransaction(TransactionAuthorizationRequest transactionRequest)
    {
        var invalidTransaction = ValidateTransaction(transactionRequest.Amount);
        if (invalidTransaction.Equals(default))
        {
            return new TransactionAuthorizationResponse(transactionRequest.TransactionId, Authorized: true, Code: "10000", Description: "Successful");
        }

        return new TransactionAuthorizationResponse(transactionRequest.TransactionId, Authorized: false, invalidTransaction.TransactionCode, invalidTransaction.Description);
    }

    private static (string AmountEndWith, string TransactionCode, string Description) ValidateTransaction(decimal amount) =>
        InvalidTransactions.FirstOrDefault(x => amount.ToString().EndsWith(x.AmountEndWith));

    /// <summary>
    /// https://www.checkout.com/docs/resources/codes/response-codes#Overview
    /// </summary>
    private static readonly List<(string AmountEndWith, string TransactionCode, string Description)> InvalidTransactions = new()
    {
        ("05", "20005", "Declined - Do not honour"),
        ("12", "20012", "Invalid transaction"),
        ("14", "20014", "Invalid card number"),
        ("51", "20051", "Insufficient funds"),
        ("54", "20087", "Bad track data"),
        ("62", "20062", "Restricted card"),
        ("63", "20063", "Security violation"),
        ("9998", "20068", "Response received too late / timeout"),
        ("150", "20150", "Card not 3D Secure enabled"),
        ("6900", "20150", "Unable to specify if card is 3D Secure enabled"),
        ("5000", "20153", "3D Secure system malfunction"),
        ("5029", "20153", "3D Secure system malfunction"),
        ("6510", "20154", "3D Secure Authentication Required"),
        ("6520", "20154", "3D Secure Authentication Required"),
        ("6530", "20154", "3D Secure Authentication Required"),
        ("6540", "20154", "3D Secure Authentication Required"),
        ("33", "30033", "Expired card - Pick up"),
        ("4001", "40101", "Payment blocked due to risk"),
        ("4008", "40108", "Gateway Reject – Post code failed"),
        ("2011", "200R1", "Issuer initiated a stop payment (revocation order) for this authorization"),
        ("2013", "200R3", "Issuer initiated a stop payment (revocation order) for all payments")
    };
}
