using Checkout.Domain.Transaction;

namespace Checkout.Query.Application.Interfaces
{
    public interface ITransactionsHistoryQueryRepository
    {
        Task<Transaction> GetByTransactionIdAsync(Guid transactionId);
        Task<List<Transaction>> GetByMerchantIdAsync(Guid merchantId);
    }
}
