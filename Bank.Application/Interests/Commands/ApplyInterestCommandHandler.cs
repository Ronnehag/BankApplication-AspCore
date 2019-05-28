using System;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using Bank.Application.Interfaces;
using Bank.Common;
using Bank.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Interests.Commands
{
    public class ApplyInterestCommandHandler : IRequestHandler<ApplyInterestsCommand, Unit>
    {
        private readonly IBankDbContext _context;
        private readonly IDateTime _time;

        public ApplyInterestCommandHandler(IBankDbContext context, IDateTime time)
        {
            _context = context;
            _time = time;
        }

        public async Task<Unit> Handle(ApplyInterestsCommand request, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == request.AccountId, cancellationToken);
            if (account == null)
            {
                throw new NotFoundException(nameof(account), request.AccountId);
            }
            if (account.Balance != null && account.Balance.Value > 0)
            {
                var timeSpanDays = (_time.Now - request.LastCalculatedDate).Days;
                var dailyInterest = ((request.APR / 100) / Interest.DaysAYear) * account.Balance.Value;
                var totalInterestSum = Math.Round(dailyInterest * timeSpanDays, 2);
                account.Balance += totalInterestSum;
                var transaction = new Transaction
                {
                    Type = TransactionType.Credit,
                    Balance = account.Balance.Value,
                    AccountId = account.AccountId,
                    Date = _time.Now,
                    Amount = totalInterestSum,
                    Operation = Operation.Credit,
                    Symbol = Interest.Symbol
                };
                _context.Accounts.Update(account);
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}
