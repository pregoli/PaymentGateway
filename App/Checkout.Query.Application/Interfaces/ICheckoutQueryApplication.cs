using Checkout.Query.Application.Dtos;

namespace Checkout.Query.Application.Interfaces;

public interface ICheckoutQueryApplication
{
    Task<TransactionResponse> GetTransactionByIdAsync(Guid transactionId);
    Task<List<TransactionResponse>> GetTransactionsByMerchantIdAsync(Guid merchantId);
}
