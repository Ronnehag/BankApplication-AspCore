using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<IResult>
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
