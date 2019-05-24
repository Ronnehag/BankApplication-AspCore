using System.ComponentModel.DataAnnotations;
using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Accounts.Commands.CreateAccountDebit
{
    public class CreateAccountDebitCommand : IRequest<IResult>
    {
        [Required(ErrorMessage = "Add an account number")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Add an amount")]
        [Range(1.00, 99999999999.99, ErrorMessage = "Can't enter a negative amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
