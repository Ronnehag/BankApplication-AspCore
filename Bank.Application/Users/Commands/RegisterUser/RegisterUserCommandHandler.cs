using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bank.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResult>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterUserCommandHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email
            };
            var create = await _userManager.CreateAsync(user, request.Password);
            if (create.Succeeded)
            {
                switch (request.SelectedRole)
                {
                    case Claims.Admin:
                        await _userManager.AddClaimAsync(user, new Claim(Claims.Admin, "true"));
                        break;

                    case Claims.Cashier:
                        await _userManager.AddClaimAsync(user, new Claim(Claims.Cashier, "true"));
                        break;
                }
                return new RegisterUserResult { IsSuccess = true };
            }
            return new RegisterUserResult { IsSuccess = false };
        }
    }
}
