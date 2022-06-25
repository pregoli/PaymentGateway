using Checkout.Command.Application.Dtos;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Command.Application.Events;

internal record PaymentExecuted : INotification
{
    public PaymentExecuted(Transaction? transaction)
    {
        Transaction = transaction;
    }

    internal Transaction? Transaction { get; private init; }
}

internal class PaymentExecutedHandler : INotificationHandler<PaymentExecuted>
{
    private readonly IAcquiringBankProvider _acquiringBankProvider;
    private readonly ITransactionsWriteRepository _transactionsWriteRepository;
    private readonly ILogger<PaymentExecutedHandler> _logger;

    public PaymentExecutedHandler(
        IAcquiringBankProvider acquiringBankProvider,
        ITransactionsWriteRepository transactionsWriteRepository,
        ILogger<PaymentExecutedHandler> logger)
    {
        _acquiringBankProvider = acquiringBankProvider;
        _transactionsWriteRepository = transactionsWriteRepository;
        _logger = logger;
    }

    public async Task Handle(PaymentExecuted @event, CancellationToken cancellationToken)
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