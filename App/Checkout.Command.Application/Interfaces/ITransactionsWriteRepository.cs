using Checkout.Domain.Transaction;

namespace Checkout.Command.Application.Interfaces;

public interface ITransactionsWriteRepository
{
    Task<Transaction> SaveAsync(Transaction transaction);
    Task<Transaction> UpdateAsync(Transaction transaction);
}
