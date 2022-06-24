using Checkout.Query.Application.Dtos;

namespace Checkout.Query.Application.Interfaces;

public interface ICheckoutQueryApplication
{
    Task<TransactionResponse> GetTransactionById(Guid transactionId);
    Task<List<TransactionResponse>> GetTransactionsByMerchantId(Guid merchantId);
}
