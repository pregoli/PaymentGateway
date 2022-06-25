using Checkout.Command.Application.Dtos;

namespace Checkout.Api.Requests;

public class TransactionRequest
{
    public Guid MerchantId { get; set; }

    public CardDetailsDto CardDetails { get; set; }

    public decimal Amount { get; set; }
}
