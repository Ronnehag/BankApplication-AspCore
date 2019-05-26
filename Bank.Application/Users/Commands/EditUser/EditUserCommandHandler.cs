using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Interfaces;
using Bank.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Bank.Application.Users.Commands.EditUser
{
    public class EditUserCommandHandler : IRequestHandler<EditUserCommand, IResult>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public EditUserCommandHandler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IResult> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return new BaseResult { IsSuccess = false, Error = "User not found" };
            }
            var updateResult = new BaseResult();
            if (!string.IsNullOrWhiteSpace(request.NewEmail))
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);
                var result = await _userManager.ChangeEmailAsync(user, request.NewEmail, token);
                if (result.Succeeded)
                {
                    updateResult.IsSuccess = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var reset = await _userManager.ResetPasswordAsync(user, token, request.Password);
                if (reset.Succeeded)
                {
                    updateResult.IsSuccess = true;
                }
            }
            if (updateResult.IsSuccess)
            {
                updateResult.Success = "User updated successfully";
            }
            else
            {
                updateResult.Error = "An error occured while updating the user, try again shortly";
            }
            return updateResult;
        }
    }
}
