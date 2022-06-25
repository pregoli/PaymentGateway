using Checkout.Query.Application.Dtos;
using Checkout.Query.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Query.Application.Queries;

public class GetTransactionsByMerchantId : IRequest<List<TransactionResponse>>
{
    public GetTransactionsByMerchantId(Guid merchantId)
    {
        MerchantId = merchantId;
    }

    public Guid MerchantId { get; }
}

public class GetTransactionsByMerchantIdQueryHandler : IRequestHandler<GetTransactionsByMerchantId, List<TransactionResponse>>
{
    private readonly ITransactionsQueryRepository _transactionsQueryRepository;
    private readonly ILogger<GetTransactionsByMerchantIdQueryHandler> _logger;

    public GetTransactionsByMerchantIdQueryHandler(ITransactionsQueryRepository transactionsQueryRepository, ILogger<GetTransactionsByMerchantIdQueryHandler> logger)
    {
        _transactionsQueryRepository = transactionsQueryRepository;
        _logger = logger;
    }

    public async Task<List<TransactionResponse>> Handle(GetTransactionsByMerchantId request, CancellationToken cancellationToken)
    {
        var response = new List<TransactionResponse>();

        try
        {
            var transactions = await _transactionsQueryRepository.GetByMerchantIdAsync(request.MerchantId);
            return transactions.Select(transaction => TransactionResponse.Map(
                transaction.Id,
                transaction.MerchantId,
                transaction.CardDetails,
                transaction.Amount,
                transaction.Status.ToString(),
                transaction.Description,
                transaction.Timestamp)).ToList()!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Checkout Request: Unhandled Exception for Request {@Request}", request);
        }

        return response;
    }
}