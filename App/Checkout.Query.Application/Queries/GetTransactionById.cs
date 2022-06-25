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

public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionById, TransactionResponse?>
{
    private readonly ITransactionsQueryRepository _transactionsQueryRepository;
    private readonly ILogger<GetTransactionByIdQueryHandler> _logger;

    public GetTransactionByIdQueryHandler(ITransactionsQueryRepository transactionsQueryRepository, ILogger<GetTransactionByIdQueryHandler> logger)
    {
        _transactionsQueryRepository = transactionsQueryRepository;
        _logger = logger;
    }

    public async Task<TransactionResponse?> Handle(GetTransactionById request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionsQueryRepository.GetByTransactionIdAsync(request.Id);
        if (transaction is not null)
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