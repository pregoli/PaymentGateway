using Checkout.Domain.Transaction;

namespace Checkout.Query.Application.Interfaces
{
    public interface ITransactionsQueryRepository
    {
        Task<Transaction?> GetByTransactionIdAsync(Guid transactionId);
        Task<List<Transaction>> GetByMerchantIdAsync(Guid merchantId);
    }
}
