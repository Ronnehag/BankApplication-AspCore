using System.Threading;
using System.Threading.Tasks;
using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Interfaces
{
    public interface IBankDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Card> Cards { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Disposition> Dispositions { get; set; }
        DbSet<Loan> Loans { get; set; }
        DbSet<PermenentOrder> PermenentOrder { get; set; }
        DbSet<Transaction> Transactions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
