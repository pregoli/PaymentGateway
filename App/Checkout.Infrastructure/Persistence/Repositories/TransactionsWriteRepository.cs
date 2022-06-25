using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;

namespace Checkout.Infrastructure.Persistence.Repositories;

public class TransactionsWriteRepository : ITransactionsWriteRepository
{
    private readonly CheckoutDbContext _context;
    public TransactionsWriteRepository(CheckoutDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction> SaveAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return transaction;
    }

    public async Task<Transaction> UpdateAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();

        return transaction;
    }
}
