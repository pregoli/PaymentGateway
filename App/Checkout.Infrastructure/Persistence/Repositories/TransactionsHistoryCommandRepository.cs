using System;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Command.Application.Interfaces;
using Checkout.Domain.Transaction;
using Microsoft.Extensions.Caching.Memory;

namespace Checkout.Infrastructure.Persistence.Repositories
{
    public class TransactionsHistoryCommandRepository : ITransactionsHistoryCommandRepository
    {
        private readonly CheckoutDbContext context;
        public TransactionsHistoryCommandRepository(CheckoutDbContext context)
        {
            this.context = context;
        }

        public async Task<Transaction> SaveAsync(Transaction transaction)
        {
            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            return transaction;
        }

        public async Task<Transaction> UpdateAsync(Transaction transaction)
        {
            context.Transactions.Update(transaction);
            await context.SaveChangesAsync();

            return transaction;
        }
    }
}
