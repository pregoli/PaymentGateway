using Checkout.Domain.Transaction;
using Checkout.Query.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Infrastructure.Persistence.Repositories;

public class TransactionsQueryRepository : ITransactionsQueryRepository
{
    private readonly CheckoutDbContext _context;
    public TransactionsQueryRepository(CheckoutDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction?> GetByTransactionIdAsync(Guid transactionId)
    {
        return await _context.Transactions.FindAsync(transactionId);
    }

    public async Task<IReadOnlyList<Transaction>> GetByMerchantIdAsync(Guid merchantId)
    {
        return await _context.Transactions.Where(x => x.MerchantId == merchantId).ToListAsync();
    }
}