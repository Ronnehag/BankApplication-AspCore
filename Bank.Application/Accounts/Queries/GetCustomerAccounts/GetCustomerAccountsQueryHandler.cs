using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Accounts.Queries.GetCustomerAccounts
{
    public class GetCustomerAccountsQueryHandler : IRequestHandler<GetCustomerAccountsQuery, IEnumerable<CustomerAccountDto>>
    {
        private readonly IBankDbContext _context;

        public GetCustomerAccountsQueryHandler(IBankDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerAccountDto>> Handle(GetCustomerAccountsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Dispositions
                    .Include(d => d.Account)
                    .Where(a => a.CustomerId == request.CustomerId)
                    .Select(c => new CustomerAccountDto
                    {
                        AccountId = c.AccountId,
                        Balance = c.Account.Balance ?? 0
                    }).AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
