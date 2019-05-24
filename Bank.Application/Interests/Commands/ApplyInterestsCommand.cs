using System;
using Bank.Application.Enumerations;
using MediatR;

namespace Bank.Application.Interests.Commands
{
    public class ApplyInterestsCommand : IRequest<Unit>
    {
        public int AccountId { get; set; }
        public decimal APR { get; set; } = Interest.Rate;
        public DateTime LastCalculatedDate { get; set; }
    }
}
