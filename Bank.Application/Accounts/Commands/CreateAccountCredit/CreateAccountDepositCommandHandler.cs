using System;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using Bank.Application.Interfaces;
using Bank.Application.Models;
using Bank.Common;
using Bank.Common.Extensions;
using Bank.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Accounts.Commands.CreateAccountCredit
{
    public class CreateAccountDepositCommandHandler : IRequestHandler<CreateAccountDepositCommand, IResult>
    {
        private readonly IBankDbContext _context;
        private readonly IDateTime _time;

        public CreateAccountDepositCommandHandler(IBankDbContext context, IDateTime time)
        {
            _context = context;
            _time = time;
        }

        public async Task<IResult> Handle(CreateAccountDepositCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount < 0)
            {
                throw new NegativeAmountException(nameof(Account), request.AccountId);
            }

            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.AccountId == request.AccountId, cancellationToken);
            if (account == null)
            {
                throw new NotFoundException(nameof(Account), request.AccountId);
            }

            request.Amount = Math.Round(request.Amount, 2);
            account.Balance += request.Amount;

            var transaction = new Transaction
            {
                AccountId = account.AccountId,
                Balance = Math.Round(account.Balance ?? 0, 2),
                Date = _time.Now,
                Type = TransactionType.Credit,
                Amount = request.Amount,
                Operation = Operation.Credit
            };

            _context.Accounts.Update(account);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);
            return new BaseResult
            {
                IsSuccess = true,
                Success = $"Successfully deposited {request.Amount.ToSwedishKrona()} to account #{request.AccountId}"
            };

        }
    }
}
