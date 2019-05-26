using System.ComponentModel.DataAnnotations;
using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Users.Commands.EditUser
{
    public class EditUserCommand : IRequest<IResult>
    {
        public string UserId { get; set; }
        public string Email { get; set; }

        public string NewEmail { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
