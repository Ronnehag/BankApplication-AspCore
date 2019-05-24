using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Exceptions;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Accounts.Queries.GetAccountDetails
{
    public class GetAccountDetailQueryHandler : IRequestHandler<GetAccountDetailQuery, AccountDetailDto>
    {
        private readonly IBankDbContext _context;

        public GetAccountDetailQueryHandler(IBankDbContext context)
        {
            _context = context;
        }

        public async Task<AccountDetailDto> Handle(GetAccountDetailQuery request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .Include(a => a.Loans)
                .Include(a => a.PermenentOrders)
                .SingleOrDefaultAsync(a => a.AccountId == request.AccountId, cancellationToken);
            if (account == null)
            {
                throw new NotFoundException(nameof(Account), request.AccountId);
            }

            return new AccountDetailDto
            {
                AccountId = account.AccountId,
                Balance = account.Balance,
                Dispositions = account.Dispositions.Select(d => new DispositionDto
                {
                    Type = d.Type,
                    CustomerId = d.CustomerId
                }).ToList(),
                Loans = account.Loans.Select(l => new LoanDto
                {
                    Status = l.Status,
                    Amount = l.Amount,
                    Duration = l.Duration,
                    Date = l.Date,
                    Payments = l.Payments,
                    LoanId = l.LoanId
                }).ToList(),
                PermanentOrders = account.PermenentOrders.Select(p => new PermanentOrderDto
                {
                    Id = p.OrderId,
                    AccountTo = p.AccountTo,
                    Amount = p.Amount,
                    Bank = p.BankTo,
                    Symbol = p.Symbol
                }).ToList()
            };
        }
    }
}
