using Checkout.Domain.Transaction.ValueObjects;

namespace Checkout.Api.Requests;

public class TransactionRequest
{
    public Guid MerchantId { get; set; }

    public CardDetails CardDetails { get; set; }

    public decimal Amount { get; set; }
}
