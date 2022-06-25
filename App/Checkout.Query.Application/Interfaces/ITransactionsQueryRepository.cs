using Checkout.Domain.Transaction;

namespace Checkout.Query.Application.Interfaces
{
    public interface ITransactionsQueryRepository
    {
        Task<Transaction?> GetByTransactionIdAsync(Guid transactionId);
        Task<IReadOnlyList<Transaction>> GetByMerchantIdAsync(Guid merchantId);
    }
}
