using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Enumerations;
using Bank.Application.Interfaces;
using Bank.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bank.Application.Users.Commands.ChangeClaim
{
    public class ChangeClaimCommandHandler : IRequestHandler<ChangeClaimCommand, IResult>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ChangeClaimCommandHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> Handle(ChangeClaimCommand request, CancellationToken cancellationToken)
        {
            if (request.NewClaim == request.CurrentClaim)
            {
                return new BaseResult { IsSuccess = false, Error = "User already has that authority." };
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new BaseResult { IsSuccess = false, Error = "User not found." };
            }

            var claimToRemove = _userManager.GetClaimsAsync(user).Result.Single(c => c.Type == request.CurrentClaim);
            var removedResult = await _userManager.RemoveClaimAsync(user, claimToRemove);
            if (removedResult.Succeeded)
            {
                var addClaim = await _userManager.AddClaimAsync(user, new Claim(request.NewClaim, "true"));
                if (addClaim.Succeeded)
                {
                    return new BaseResult { IsSuccess = true };
                }
            }
            return new BaseResult { IsSuccess = false, Error = "An error occured while removing the current claim." };
        }
    }
}
