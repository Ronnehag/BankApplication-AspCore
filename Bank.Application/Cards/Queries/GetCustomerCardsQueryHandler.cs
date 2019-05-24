using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Cards.Queries
{
    public class GetCustomerCardsQueryHandler : IRequestHandler<GetCustomerCardsQuery, IEnumerable<CardDto>>
    {
        private readonly IBankDbContext _context;

        public GetCustomerCardsQueryHandler(IBankDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<CardDto>> Handle(GetCustomerCardsQuery request, CancellationToken cancellationToken)
        {
            var dispositionId = _context.Dispositions
                .Where(c => c.CustomerId == request.CustomerId)
                .Select(c => c.DispositionId)
                .FirstOrDefault();
            if (dispositionId == 0)
            {
                return new List<CardDto>();
            }
            return await _context.Cards
                .Where(c => c.DispositionId == dispositionId)
                .Select(c => new CardDto
                {
                    Type = c.Type,
                    CcNumber = c.Ccnumber,
                    CcType = c.Cctype,
                    ExpM = c.ExpM,
                    ExpY = c.ExpY,
                    Issued = c.Issued
                }).AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
