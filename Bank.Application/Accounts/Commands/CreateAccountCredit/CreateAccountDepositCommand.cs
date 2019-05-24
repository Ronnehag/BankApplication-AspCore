using System.ComponentModel.DataAnnotations;
using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Accounts.Commands.CreateAccountCredit
{
    public class CreateAccountDepositCommand : IRequest<IResult>
    {
        [Required(ErrorMessage = "Add account number")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Add the amount")]
        [Range(1.0, 99999999999.99, ErrorMessage = "Can't enter a negative amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
