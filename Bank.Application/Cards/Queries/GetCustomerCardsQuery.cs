using System.Collections.Generic;
using MediatR;

namespace Bank.Application.Cards.Queries
{
    public class GetCustomerCardsQuery : IRequest<IEnumerable<CardDto>>
    {
        public int CustomerId { get; set; }
    }
}
