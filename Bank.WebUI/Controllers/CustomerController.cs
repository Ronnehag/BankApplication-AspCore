using System.Threading.Tasks;
using Bank.Application.Customers.Commands.CreateCustomer;
using Bank.Application.Customers.Commands.UpdateCustomer;
using Bank.Application.Customers.Queries.GetCustomer;
using Bank.Application.Customers.Queries.GetCustomerDetails;
using Bank.Application.Customers.Queries.GetCustomerList;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebUI.Controllers
{
    [Authorize(Policy = Claims.Cashier)]
    public class CustomerController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(GetCustomerListQuery query)
        {
            query.Offset = query.Limit * (query.CurrentPage - 1);
            return View(await Mediator.Send(query));
        }

        public async Task<IActionResult> CustomerDetails(int id)
        {
            try
            {
                return View(await Mediator.Send(new GetCustomerDetailsQuery { CustomerId = id }));
            }
            catch (NotFoundException)
            {
                TempData["Error"] = $"Customer ID {id} not found.";
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm]CreateCustomerCommand query)
        {
            if (ModelState.IsValid)
            {
                var result = await Mediator.Send(query);
                if (result.IsSuccess)
                {
                    TempData["Message"] = result.Success;
                    return RedirectToAction(nameof(Register));
                }
                TempData["Error"] = result.Error;
            }
            return View(query);
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var customer = await Mediator.Send(new GetCustomerQuery { CustomerId = id });
                return View(new UpdateCustomerCommand { Profile = customer });
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCustomerCommand command)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(command);
                    if (result.IsSuccess)
                    {
                        TempData["Message"] = result.Success;
                    }

                    return RedirectToAction(nameof(CustomerDetails), new { id = command.Profile.CustomerId });
                }
                catch (NotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
            }
            return View(command);
        }
    }
}