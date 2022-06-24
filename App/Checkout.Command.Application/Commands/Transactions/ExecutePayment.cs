using System;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Command.Application.Events.Transactions;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;
using Checkout.Domain.Transaction.ValueObjects;
using MediatR;

namespace Checkout.Command.Application.Commands.Transactions;

public class ExecutePayment : IRequest<Guid>
{
    public ExecutePayment(Guid merchantId, CardDetails cardDetails, decimal amount)
    {
        MerchantId = merchantId;
        CardDetails = cardDetails;
        Amount = amount;
    }

    internal Guid MerchantId { get; }
    internal CardDetails CardDetails { get; }
    internal decimal Amount { get; }
}

internal class ExecutePaymentHandler : IRequestHandler<ExecutePayment, Guid>
{
    private readonly ITransactionsHistoryCommandRepository _transactionsHistoryCommandRepository;
    private readonly IPublisher _publisher;

    public ExecutePaymentHandler(ITransactionsHistoryCommandRepository transactionsHistoryCommandRepository, IPublisher publisher)
    {
        _transactionsHistoryCommandRepository = transactionsHistoryCommandRepository;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(ExecutePayment command, CancellationToken cancellationToken)
    {
        var transaction = Transaction.Create(command.MerchantId, command.Amount, command.CardDetails);
        await _transactionsHistoryCommandRepository.SaveAsync(transaction);

        _publisher.Publish(new PaymentExecuted(transaction), cancellationToken);

        return transaction.Id;
    }
}