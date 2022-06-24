using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Checkout.Command.Application.Common.Dto;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;
using Checkout.Domain.Transaction.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Command.Application.Events.Transactions;

internal record PaymentExecuted : INotification
{
    public PaymentExecuted(Transaction transaction)
    {
        Transaction = transaction;
    }

    internal Transaction Transaction { get; private init; }
}

internal class PaymentExecutedHandler : INotificationHandler<PaymentExecuted>
{
    private readonly IAcquiringBankProvider _acquiringBankProvider;
    private readonly ITransactionsHistoryCommandRepository _transactionsHistoryCommandRepository;
    private readonly ILogger<PaymentExecutedHandler> _logger;

    public PaymentExecutedHandler(
        IAcquiringBankProvider acquiringBankProvider,
        ITransactionsHistoryCommandRepository transactionsHistoryCommandRepository,
        ILogger<PaymentExecutedHandler> logger)
    {
        _acquiringBankProvider = acquiringBankProvider;
        _transactionsHistoryCommandRepository = transactionsHistoryCommandRepository;
        _logger = logger;
    }

    public async Task Handle(PaymentExecuted @event, CancellationToken cancellationToken)
    {
        try
        {
            var transaction = @event.Transaction;

            var transactionResponse = _acquiringBankProvider.ValidateTransaction(
                new TransactionAuthorizationRequest(transaction.Id, transaction.MerchantId, transaction.Amount));

            if (transactionResponse.Authorized)
                transaction.Authorize();
            else
                transaction.Reject(transactionResponse.Description);

            await _transactionsHistoryCommandRepository.UpdateAsync(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled Exception for Event {Event}", @event);
            throw;
        }
    }
}