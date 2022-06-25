using Checkout.Command.Application.Dtos;
using Checkout.Command.Application.Events;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;
using Checkout.Domain.Transaction.ValueObjects;
using MediatR;

namespace Checkout.Command.Application.Commands;

public class SubmitPayment : IRequest<Guid>
{
    public SubmitPayment(Guid merchantId, CardDetailsDto cardDetails, decimal amount)
    {
        MerchantId = merchantId;
        CardDetails = cardDetails;
        Amount = amount;
    }

    public Guid MerchantId { get; }
    public CardDetailsDto CardDetails{ get; }
    public decimal Amount { get; }
}

internal class SubmitPaymenthandler : IRequestHandler<SubmitPayment, Guid>
{
    private readonly ITransactionsWriteRepository _transactionsWriteRepository;
    private readonly IPublisher _publisher;

    public SubmitPaymenthandler(ITransactionsWriteRepository transactionsWriteRepository, IPublisher publisher)
    {
        _transactionsWriteRepository = transactionsWriteRepository;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(SubmitPayment command, CancellationToken cancellationToken)
    {
        var creditCard = CardDetails.Create(command.CardDetails.HolderName, command.CardDetails.Number, command.CardDetails.ExpirationMonth, command.CardDetails.ExpirationYear, command.CardDetails.Cvv);
        var transaction = Transaction.Create(command.MerchantId, command.Amount, creditCard);
        
        await _transactionsWriteRepository.SaveAsync(transaction!);

        _publisher.Publish(new PaymentSubmitted(transaction), cancellationToken);

        return transaction!.Id;
    }
}