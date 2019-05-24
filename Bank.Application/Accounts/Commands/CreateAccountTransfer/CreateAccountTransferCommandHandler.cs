using System;
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

namespace Bank.Application.Accounts.Commands.CreateAccountTransfer
{
    public class CreateAccountTransferCommandHandler : IRequestHandler<CreateAccountTransferCommand, IResult>
    {
        private readonly IBankDbContext _context;
        private readonly IDateTime _time;

        public CreateAccountTransferCommandHandler(IBankDbContext context, IDateTime time)
        {
            _context = context;
            _time = time;
        }

        public async Task<IResult> Handle(CreateAccountTransferCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount < 0)
            {
                throw new NegativeAmountException(nameof(Account), request.AccountIdFrom);
            }

            var accountFrom = await _context.Accounts
                    .SingleOrDefaultAsync(a => a.AccountId == request.AccountIdFrom,
                    cancellationToken);
            var accountTo = await _context.Accounts
                .SingleOrDefaultAsync(a => a.AccountId == request.AccountIdTo,
                    cancellationToken);

            if (accountFrom == null)
            {
                throw new NotFoundException(nameof(Account), request.AccountIdFrom);
            }
            if (accountTo == null)
            {
                throw new NotFoundException(nameof(Account), request.AccountIdTo);
            }
            if (accountFrom.Balance != null && accountFrom.Balance.Value - request.Amount < 0)
            {
                throw new InsufficientFundsException(nameof(Account), request.AccountIdFrom);
            }
            request.Amount = Math.Round(request.Amount, 2);

            accountFrom.Balance -= request.Amount;
            accountTo.Balance += request.Amount;
            _context.Accounts.UpdateRange(accountFrom, accountTo);

            var transactionFrom = new Transaction
            {
                AccountId = request.AccountIdFrom,
                Balance = Math.Round(accountFrom.Balance.Value, 2),
                Amount = -request.Amount,
                Date = _time.Now,
                Type = TransactionType.Debit,
                Operation = Operation.TransferDebit
            };
            var transactionTo = new Transaction
            {
                AccountId = request.AccountIdTo,
                Balance = Math.Round(accountTo.Balance.Value, 2),
                Amount = request.Amount,
                Operation = Operation.Transfer,
                Type = TransactionType.Credit,
                Date = _time.Now,
            };
            await _context.Transactions.AddRangeAsync(transactionFrom, transactionTo);

            await _context.SaveChangesAsync(cancellationToken);
            return new BaseResult
            {
                IsSuccess = true,
                Success = $"Successfully transfered {request.Amount.ToSwedishKrona()} to account #{request.AccountIdTo}."
            };
        }
    }
}
