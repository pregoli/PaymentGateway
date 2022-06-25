using Checkout.Command.Application.Dtos;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Command.Application.Events;

internal record PaymentSubmitted : INotification
{
    public PaymentSubmitted(Transaction? transaction)
    {
        Transaction = transaction;
    }

    internal Transaction? Transaction { get; private init; }
}

internal class PaymentSubmittedHandler : INotificationHandler<PaymentSubmitted>
{
    private readonly IAcquiringBankProvider _acquiringBankProvider;
    private readonly ITransactionsWriteRepository _transactionsWriteRepository;
    private readonly ILogger<PaymentSubmittedHandler> _logger;

    public PaymentSubmittedHandler(
        IAcquiringBankProvider acquiringBankProvider,
        ITransactionsWriteRepository transactionsWriteRepository,
        ILogger<PaymentSubmittedHandler> logger)
    {
        _acquiringBankProvider = acquiringBankProvider;
        _transactionsWriteRepository = transactionsWriteRepository;
        _logger = logger;
    }

    public async Task Handle(PaymentSubmitted @event, CancellationToken cancellationToken)
    {
        try
        {
            var transaction = @event.Transaction!;

            var transactionResponse = _acquiringBankProvider.ValidateTransaction(
                new TransactionAuthorizationRequest(transaction.Id, transaction.MerchantId, transaction.Amount));

            if (transactionResponse.Authorized)
                transaction.Authorize();
            else
                transaction.Reject(transactionResponse.Description);

            await _transactionsWriteRepository.UpdateAsync(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled Exception for Event {Event}", @event);
            throw;
        }
    }
}