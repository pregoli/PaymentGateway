using Checkout.Command.Application.Dtos;

namespace Checkout.Command.Application.Interfaces;

public interface ICheckoutCommandApplication
{
    Task<Guid> SubmitPayment(Guid merchantId, CardDetailsDto CardDetails, decimal amount);
}