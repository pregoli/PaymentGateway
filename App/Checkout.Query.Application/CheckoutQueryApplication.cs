using Checkout.Query.Application.Dtos;
using Checkout.Query.Application.Interfaces;
using Checkout.Query.Application.Queries.Transactions;
using MediatR;

namespace Checkout.Query.Application;

internal class CheckoutQueryApplication : ICheckoutQueryApplication
{
    private readonly ISender _sender;
    public CheckoutQueryApplication(ISender sender)
    {
        _sender = sender;
    }

    public async Task<TransactionResponse> GetTransaction(Guid transactionId)
    {
        return await _sender.Send(new GetTransactionById(transactionId));
    }

    public async Task<List<TransactionResponse>> GetTransactionsByMerchantId(Guid merchantId)
    {
        return await _sender.Send(new GetTransactionByMerchantId(merchantId));
    }
}