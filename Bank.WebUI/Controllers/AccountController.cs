﻿using System.Threading.Tasks;
using Bank.Application.Accounts.Commands.CreateAccountCredit;
using Bank.Application.Accounts.Commands.CreateAccountDebit;
using Bank.Application.Accounts.Commands.CreateAccountTransfer;
using Bank.Application.Accounts.Queries.GetAccountDetails;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebUI.Controllers
{
    [Authorize(Policy = Claims.Cashier)]
    public class AccountController : BaseController
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit([FromForm]CreateAccountDepositCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(command);
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
            TempData["Error"] = "You entered an invalid amount. Amount can't be zero, negative or over 13 digits.";
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
                    var result = await Mediator.Send(command);
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
            TempData["Error"] = "You entered an invalid amount. Amount can't be zero, negative or over 13 digits.";
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
                    var result = await Mediator.Send(command);
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
            TempData["Error"] = "You entered an invalid amount. Amount can't be zero, negative or over 13 digits.";
            return RedirectToAction(nameof(Details), new { id = command.AccountIdFrom });
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                return View(await Mediator.Send(new GetAccountDetailQuery { AccountId = id }));
            }
            catch (NotFoundException)
            {
                return NotFound(id);
            }
        }
    }
}