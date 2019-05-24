using System.Threading.Tasks;
using Bank.Application.Accounts.Commands.CreateAccountCredit;
using Bank.Application.Accounts.Commands.CreateAccountDebit;
using Bank.Application.Accounts.Commands.CreateAccountTransfer;
using Bank.Application.Accounts.Queries.GetAccountDetails;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using Bank.Application.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebUI.Controllers
{
    [Authorize(Policy = Claims.Cashier)]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit([FromForm]CreateAccountDepositCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _mediator.Send(command);
                    TempData["Message"] = result.Success;
                }
                catch (NotFoundException ex)
                {
                    TempData["Error"] = ex.Message;
                }
                catch (NegativeAmountException ex)
                {
                    TempData["Error"] = ex.Message;
                }
                return RedirectToAction(nameof(Details), new { id = command.AccountId });
            }
            TempData["Error"] = "That amount is too large or too low to be deposited.";
            return RedirectToAction(nameof(Details), new { id = command.AccountId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Debit([FromForm] CreateAccountDebitCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _mediator.Send(command);
                    TempData["Message"] = result.Success;
                }
                catch (NotFoundException ex)
                {
                    TempData["Error"] = ex.Message;
                }
                catch (InsufficientFundsException ex)
                {
                    TempData["Error"] = ex.Message;
                }
                catch (NegativeAmountException ex)
                {
                    TempData["Error"] = ex.Message;
                }
                return RedirectToAction(nameof(Details), new { id = command.AccountId });
            }
            TempData["Error"] = "That amount is too large or too low to be debited.";
            return RedirectToAction(nameof(Details), new { id = command.AccountId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer([FromForm] CreateAccountTransferCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _mediator.Send(command);
                    TempData["Message"] = result.Success;
                }
                catch (NotFoundException ex)
                {
                    TempData["Error"] = ex.Message;
                }
                catch (InsufficientFundsException ex)
                {
                    TempData["Error"] = ex.Message;
                }
                catch (NegativeAmountException ex)
                {
                    TempData["Error"] = ex.Message;
                }
                return RedirectToAction(nameof(Details), new { id = command.AccountIdFrom });
            }
            TempData["Error"] = "That amount is too large or too low to be transfered.";
            return RedirectToAction(nameof(Details), new { id = command.AccountIdFrom });
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                return View(await _mediator.Send(new GetAccountDetailQuery { AccountId = id }));
            }
            catch (NotFoundException)
            {
                return NotFound(id);
            }
        }
    }
}