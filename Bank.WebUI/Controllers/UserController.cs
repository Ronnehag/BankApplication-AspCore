using System.Threading.Tasks;
using System.Xml.XPath;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using Bank.Application.Users;
using Bank.Application.Users.Commands.ChangeClaim;
using Bank.Application.Users.Commands.DeleteUser;
using Bank.Application.Users.Commands.LoginUser;
using Bank.Application.Users.Commands.LogoutUser;
using Bank.Application.Users.Queries.GetUserList;
using Bank.WebUI.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebUI.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Policy = Claims.Admin)]
        public async Task<IActionResult> Index()
        {
            return View(await _mediator.Send(new GetUserListQuery()));
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
                    await _mediator.Send(userCommand);
                    return RedirectToAction("Index", "Home");
                }
                catch (NotFoundException)
                {
                    //TODO
                }
            }
            return View(userCommand);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _mediator.Send(new LogOutUserCommand());
            return Redirect("/");
        }

        [Authorize(Policy = Claims.Admin)]
        public IActionResult Register()
        {
            var vm = new RegisterViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Claims.Admin)]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(model.Command);
                if (result.IsSuccess)
                {
                    TempData["Message"] = $"Successfully created the user {model.Command.Email}";
                    return RedirectToAction(nameof(Register));
                }
                TempData["Message"] = $"An Error has occured while creating the user {model.Command.Email}, try using a new email or password.";
            }
            return View(model);
        }


        [AllowAnonymous]
        public IActionResult Denied()
        {
            return Challenge();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Claims.Admin)]
        public async Task<IActionResult> Edit([FromForm] UserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new ChangeClaimCommand
                {
                    NewClaim = model.SelectedClaim,
                    UserId = model.Id,
                    CurrentClaim = model.CurrentClaim
                });
                if (result.IsSuccess)
                {
                    TempData["ClaimMessage"] =
                        $"Successfully changed role from {model.CurrentClaim} to {model.SelectedClaim} for user {model.Email}";
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
        public async Task<IActionResult> Delete([FromForm] UserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(new DeleteUserCommand { UserId = model.Id, UserName = User.Identity.Name });
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
    }
}