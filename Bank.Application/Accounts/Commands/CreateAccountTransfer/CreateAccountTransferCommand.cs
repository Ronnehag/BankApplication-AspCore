using System.ComponentModel.DataAnnotations;
using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Accounts.Commands.CreateAccountTransfer
{
    public class CreateAccountTransferCommand : IRequest<IResult>
    {
        public int AccountIdTo { get; set; }

        public int AccountIdFrom { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
