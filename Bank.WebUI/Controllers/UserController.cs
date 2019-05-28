using System.Threading.Tasks;
using System.Xml.XPath;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using Bank.Application.Users;
using Bank.Application.Users.Commands.ChangeClaim;
using Bank.Application.Users.Commands.DeleteUser;
using Bank.Application.Users.Commands.EditUser;
using Bank.Application.Users.Commands.LoginUser;
using Bank.Application.Users.Commands.LogoutUser;
using Bank.Application.Users.Queries.GetUserList;
using Bank.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebUI.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        [Authorize(Policy = Claims.Admin)]
        public async Task<IActionResult> Index()
        {
            return View(await Mediator.Send(new GetUserListQuery()));
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm]LoginUserCommand userCommand)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(userCommand);
                    if (result.IsSuccess)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(nameof(userCommand.Password), result.Error);
                }
                catch (NotFoundException)
                {
                    ModelState.AddModelError(nameof(userCommand.Email), "Email not found");
                }
            }
            return View(userCommand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await Mediator.Send(new LogOutUserCommand());
            return Redirect("/");
        }

        [Authorize(Policy = Claims.Admin)]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Claims.Admin)]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(model.Command);
                if (result.IsSuccess)
                {
                    TempData["Message"] = $"Successfully created the user {model.Command.Email}";
                    return RedirectToAction(nameof(Register));
                }
                TempData["Error"] = $"An Error has occured while creating the user {model.Command.Email}, try using a new email or password.";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Claims.Admin)]
        public async Task<IActionResult> Edit([FromForm] UserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(new ChangeClaimCommand
                {
                    NewClaim = model.SelectedClaim,
                    UserId = model.Id,
                    CurrentClaim = model.CurrentClaim
                });
                if (result.IsSuccess)
                {
                    TempData["ClaimMessage"] = result.Success;
                }
                else
                {
                    TempData["ClaimMessage"] = result.Error;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Claims.Admin)]
        public async Task<IActionResult> Update([FromForm] EditUserCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(command);
                if (result.IsSuccess)
                {
                    TempData["Message"] = result.Success;
                }
                else
                {
                    TempData["Error"] = result.Error;
                }
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Claims.Admin)]
        public async Task<IActionResult> Delete([FromForm] UserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(new DeleteUserCommand { UserId = model.Id, UserName = User.Identity.Name });
                if (result.IsSuccess)
                {
                    TempData["Message"] = result.Success;
                }
                else
                {
                    TempData["Error"] = result.Error;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public IActionResult Denied()
        {
            HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return View();
        }
    }
}