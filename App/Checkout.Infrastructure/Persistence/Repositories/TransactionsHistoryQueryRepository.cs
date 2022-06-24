using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Domain.Transaction;
using Checkout.Query.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Infrastructure.Persistence.Repositories
{
    public class TransactionsHistoryQueryRepository : ITransactionsHistoryQueryRepository
    {
        private readonly CheckoutDbContext context;
        public TransactionsHistoryQueryRepository(CheckoutDbContext context)
        {
            this.context = context;
        }

        public async Task<Transaction> GetByTransactionIdAsync(Guid transactionId)
        {
            return await context.Transactions.FindAsync(transactionId);
        }

        public async Task<List<Transaction>> GetByMerchantIdAsync(Guid merchantId)
        {
            return await context.Transactions.Where(x => x.MerchantId == merchantId).ToListAsync();
        }
    }
}
