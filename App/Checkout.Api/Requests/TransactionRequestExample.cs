using Checkout.Command.Application.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.Api.Requests;

public class TransactionRequestExample : IExamplesProvider<TransactionRequest>
{
    public TransactionRequest GetExamples()
    {
        return new TransactionRequest
        {
            Amount = 100,
            MerchantId = Guid.NewGuid(),
            CardDetails = new CardDetailsDto
            {
                HolderName = "Paolo Regoli",
                Number = "4242424242424242",
                Cvv = "100",
                ExpirationMonth = "12",
                ExpirationYear = "2026"
            }
        };
    }
}