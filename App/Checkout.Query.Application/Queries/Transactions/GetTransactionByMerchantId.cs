using Checkout.Query.Application.Dtos;
using Checkout.Query.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Checkout.Query.Application.Queries.Transactions;

public class GetTransactionByMerchantId : IRequest<List<TransactionResponse>>
{
    public GetTransactionByMerchantId(Guid merchantId)
    {
        MerchantId = merchantId;
    }

    public Guid MerchantId { get; }
}

public class GetTransactionByMerchantIdQueryHandler : IRequestHandler<GetTransactionByMerchantId, List<TransactionResponse>>
{
    private readonly ITransactionsHistoryQueryRepository _transactionsHistoryQueryRepository;
    private readonly ILogger<GetTransactionByMerchantIdQueryHandler> _logger;

    public GetTransactionByMerchantIdQueryHandler(ITransactionsHistoryQueryRepository transactionsHistoryQueryRepository, ILogger<GetTransactionByMerchantIdQueryHandler> logger)
    {
        _transactionsHistoryQueryRepository = transactionsHistoryQueryRepository;
        _logger = logger;
    }

    public async Task<List<TransactionResponse>> Handle(GetTransactionByMerchantId request, CancellationToken cancellationToken)
    {
        var response = new List<TransactionResponse>();

        try
        {
            var transactions = await _transactionsHistoryQueryRepository.GetByMerchantIdAsync(request.MerchantId);
            return transactions?.Select(transaction => TransactionResponse.Map(
                transaction.Id,
                transaction.MerchantId,
                transaction.CardDetails,
                transaction.Amount,
                transaction.TransactionStatus.ToString(),
                transaction.Description,
                transaction.Timestamp)).ToList() ?? response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Checkout Request: Unhandled Exception for Request {Request}", request);
        }

        return response;
    }
}