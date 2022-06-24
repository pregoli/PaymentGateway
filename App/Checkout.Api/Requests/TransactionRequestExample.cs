using Checkout.Domain.Transaction.ValueObjects;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Checkout.Api.Requests;

public class TransactionRequestExample : IExamplesProvider<TransactionRequest>
{
    public TransactionRequest GetExamples()
    {
        return new TransactionRequest
        {
            Amount = 100,
            MerchantId = Guid.NewGuid(),
            CardDetails = new CardDetails
            {
                CardHolderName = "Paolo Regoli",
                CardNumber = "4242424242424242",
                Cvv = "100",
                ExpirationMonth = "12",
                ExpirationYear = "2026"
            }
        };
    }
}