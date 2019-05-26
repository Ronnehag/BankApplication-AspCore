using System.ComponentModel.DataAnnotations;
using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Accounts.Commands.CreateAccountCredit
{
    public class CreateAccountDepositCommand : IRequest<IResult>
    {
        public int AccountId { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
