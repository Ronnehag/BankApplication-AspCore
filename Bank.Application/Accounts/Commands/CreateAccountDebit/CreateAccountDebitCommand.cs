using System.ComponentModel.DataAnnotations;
using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Accounts.Commands.CreateAccountDebit
{
    public class CreateAccountDebitCommand : IRequest<IResult>
    {
        public int AccountId { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
