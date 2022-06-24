using Checkout.Domain.Transaction.ValueObjects;

namespace Checkout.Command.Application.Interfaces;

public interface ICheckoutCommandApplication
{
    Task<Guid> ExecutePayment(Guid merchantId, CardDetails CardDetails, decimal amount);
}