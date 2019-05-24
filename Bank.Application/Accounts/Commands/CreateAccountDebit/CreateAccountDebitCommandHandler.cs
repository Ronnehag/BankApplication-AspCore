using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using Bank.Application.Extensions;
using Bank.Application.Interfaces;
using Bank.Application.Models;
using Bank.Common;
using Bank.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Accounts.Commands.CreateAccountDebit
{
    public class CreateAccountDebitCommandHandler : IRequestHandler<CreateAccountDebitCommand, IResult>
    {
        private readonly IBankDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateAccountDebitCommandHandler(IBankDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }


        public async Task<IResult> Handle(CreateAccountDebitCommand request, CancellationToken cancellationToken)
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
            if (account.Balance != null && account.Balance.Value - request.Amount < 0)
            {
                throw new InsufficientFundsException(nameof(Account), request.AccountId);
            }
            request.Amount = Math.Round(request.Amount, 2);

            account.Balance -= request.Amount;
            var transaction = new Transaction
            {
                AccountId = account.AccountId,
                Balance = Math.Round(account.Balance.Value, 2),
                Date = _dateTime.Now,
                Amount = -request.Amount,
                Type = TransactionType.Debit,
                Operation = Operation.Withdrawal,
            };
            _context.Accounts.Update(account);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync(cancellationToken);
            return new BaseResult
            {
                IsSuccess = true,
                Success = $"Successfully debited {request.Amount.ToSwedishKrona()} from account #{request.AccountId}."
            };
        }
    }
}
