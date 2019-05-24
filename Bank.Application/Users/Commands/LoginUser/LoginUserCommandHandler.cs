using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bank.Application.Users.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResult>
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginUserCommandHandler(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException(nameof(IdentityUser), request.Email);
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);
            if (result.Succeeded)
            {
                return new LoginUserResult { IsSuccess = true };
            }

            return new LoginUserResult { IsSuccess = false };
        }
    }
}
