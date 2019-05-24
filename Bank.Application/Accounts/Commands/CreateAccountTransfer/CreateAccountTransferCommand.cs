using System.ComponentModel.DataAnnotations;
using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Accounts.Commands.CreateAccountTransfer
{
    public class CreateAccountTransferCommand : IRequest<IResult>
    {
        [Required]
        public int AccountIdTo { get; set; }

        [Required]
        public int AccountIdFrom { get; set; }

        [Required]
        [Range(1, 99999999999.99, ErrorMessage = "Can't enter a negative amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
