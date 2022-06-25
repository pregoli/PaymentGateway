using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using Checkout.Query.Application.Interfaces;
using Checkout.Query.Application.Dtos;

namespace Checkout.Query.Application.Queries;

public class GetTransactionById : IRequest<TransactionResponse>
{
    public GetTransactionById(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }
}

public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionById, TransactionResponse>
{
    private readonly ITransactionsHistoryQueryRepository _transactionsHistoryQueryRepository;
    private readonly ILogger<GetTransactionByIdQueryHandler> _logger;

    public GetTransactionByIdQueryHandler(ITransactionsHistoryQueryRepository transactionsHistoryQueryRepository, ILogger<GetTransactionByIdQueryHandler> logger)
    {
        _transactionsHistoryQueryRepository = transactionsHistoryQueryRepository;
        _logger = logger;
    }

    public async Task<TransactionResponse?> Handle(GetTransactionById request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionsHistoryQueryRepository.GetByTransactionIdAsync(request.Id);
        if (transaction != null)
        {
            return TransactionResponse.Map(
                transaction.Id,
                transaction.MerchantId,
                transaction.CardDetails,
                transaction.Amount,
                transaction.Status.ToString(),
                transaction.Description,
                transaction.Timestamp);
        }

        return null;
    }
}