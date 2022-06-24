using System;
using System.Threading.Tasks;
using Checkout.Command.Application.Commands.Transactions;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction.ValueObjects;
using MediatR;

namespace Checkout.Command.Application;

internal class CheckoutCommandApplication : ICheckoutCommandApplication
{
    private readonly ISender _sender;
    public CheckoutCommandApplication(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Guid> ExecutePayment(Guid merchantId, CardDetails CardDetails, decimal amount)
    {
        var executePayment = new ExecutePayment(merchantId, CardDetails, amount);
        return await _sender.Send(executePayment);
    }
}