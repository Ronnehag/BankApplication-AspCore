using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bank.Application.Users.Commands.LogoutUser
{
    public class LogOutUserCommandHandler : IRequestHandler<LogOutUserCommand, Unit>
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LogOutUserCommandHandler(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<Unit> Handle(LogOutUserCommand request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return Unit.Value;
        }
    }
}
