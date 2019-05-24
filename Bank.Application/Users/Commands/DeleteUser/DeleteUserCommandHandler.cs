using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Enumerations;
using Bank.Application.Interfaces;
using Bank.Application.Models;
using Bank.Application.Users.Commands.ChangeClaim;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bank.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, IResult>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly StringBuilder _builder = new StringBuilder();

        public DeleteUserCommandHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new BaseResult { Error = "User not found.", IsSuccess = false };
            }
            if (user.UserName == request.UserName)
            {
                return new BaseResult { Error = "You can't delete your own account.", IsSuccess = false };
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return new BaseResult { Success = "User deleted successfully.", IsSuccess = true };
            }
            return new BaseResult { Error = "An error occured while deleting, try again shortly.", IsSuccess = false };
        }
    }
}
