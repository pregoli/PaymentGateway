using Checkout.Domain.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Checkout.Infrastructure.Persistence
{
    public class CheckoutDbContext : DbContext
    {
        public CheckoutDbContext(DbContextOptions<CheckoutDbContext> options)
            : base(options)
        {
        }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
