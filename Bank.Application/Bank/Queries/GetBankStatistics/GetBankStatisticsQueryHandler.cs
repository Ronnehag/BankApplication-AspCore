using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Bank.Queries.GetBankStatistics
{
    public class GetBankStatisticsQueryHandler : IRequestHandler<GetBankStatisticsQuery, BankStatisticsViewModel>
    {
        private readonly IBankDbContext _context;

        public GetBankStatisticsQueryHandler(IBankDbContext context)
        {
            _context = context;
        }

        public async Task<BankStatisticsViewModel> Handle(GetBankStatisticsQuery request, CancellationToken cancellationToken)
        {
            return new BankStatisticsViewModel
            {
                TotalAccounts = await _context.Accounts.AsNoTracking().CountAsync(cancellationToken),
                TotalCustomers = await _context.Customers.AsNoTracking().CountAsync(cancellationToken),
                TotalBalance = await _context.Accounts.AsNoTracking().SumAsync(a => a.Balance ?? 0, cancellationToken)
            };
        }

    }
}
