using Checkout.Command.Application.Commands;
using Checkout.Command.Application.Dtos;
using Checkout.Command.Application.Interfaces;
using MediatR;

namespace Checkout.Command.Application;

internal class CheckoutCommandApplication : ICheckoutCommandApplication
{
    private readonly ISender _sender;
    public CheckoutCommandApplication(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Guid> SubmitPayment(Guid merchantId, CardDetailsDto CardDetails, decimal amount)
    {
        var command = new SubmitPayment(merchantId, CardDetails, amount);
        return await _sender.Send(command);
    }
}