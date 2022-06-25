using Checkout.Command.Application.Events;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;
using Checkout.Domain.Transaction.ValueObjects;
using MediatR;

namespace Checkout.Command.Application.Commands;

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
    private readonly ITransactionsWriteRepository _transactionsWriteRepository;
    private readonly IPublisher _publisher;

    public ExecutePaymentHandler(ITransactionsWriteRepository transactionsWriteRepository, IPublisher publisher)
    {
        _transactionsWriteRepository = transactionsWriteRepository;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(ExecutePayment command, CancellationToken cancellationToken)
    {
        var transaction = Transaction.Create(command.MerchantId, command.Amount, command.CardDetails);
        await _transactionsWriteRepository.SaveAsync(transaction!);

        _ = Task.Run(() => _publisher.Publish(new PaymentExecuted(transaction), cancellationToken));

        return transaction!.Id;
    }
}