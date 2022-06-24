using System;
using System.Threading.Tasks;
using Checkout.Domain.Transaction;

namespace Checkout.Command.Application.Interfaces;

public interface ITransactionsHistoryCommandRepository
{
    Task<Transaction> SaveAsync(Transaction transaction);
    Task<Transaction> UpdateAsync(Transaction transaction);
}
