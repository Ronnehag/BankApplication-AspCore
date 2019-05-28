using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Interfaces;
using Bank.Common.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Transactions.Queries.GetAccountTransactions
{
    public class GetAccountTransactionsQueryHandler : IRequestHandler<GetAccountTransactionsQuery, TransactionsViewModel>
    {
        private readonly IBankDbContext _context;

        public GetAccountTransactionsQueryHandler(IBankDbContext context)
        {
            _context = context;
        }

        public async Task<TransactionsViewModel> Handle(GetAccountTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = _context.Transactions
                .Where(t => t.AccountId == request.AccountId)
                .OrderByDescending(t => t.TransactionId)
                .AsNoTracking().AsQueryable();
                

            var totalCount = transactions.Count();

            var model = new TransactionsViewModel
            {
                Total = totalCount,
                NumberOfPages = totalCount / request.Limit - 1,
                HasMorePages = request.Offset + request.Limit < totalCount,
                Transactions = await transactions.Skip(request.Offset).Take(request.Limit).Select(t => new TransactionDto
                {
                    Balance = t.Balance.ToSwedishKrona(),
                    Operation = t.Operation,
                    Amount = t.Amount.ToSwedishKrona(),
                    Type = t.Type,
                    Date = t.Date,
                    Symbol = t.Symbol
                }).ToListAsync(cancellationToken),
            };
            return model;
        }
    }
}
