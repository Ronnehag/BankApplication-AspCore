using System.Threading.Tasks;
using Bank.Application.Customers.Commands.CreateCustomer;
using Bank.Application.Customers.Commands.UpdateCustomer;
using Bank.Application.Customers.Queries.GetCustomer;
using Bank.Application.Customers.Queries.GetCustomerDetails;
using Bank.Application.Customers.Queries.GetCustomerList;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebUI.Controllers
{
    [Authorize(Policy = Claims.Cashier)]
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(GetCustomerListQuery query)
        {
            query.Offset = query.Limit * (query.CurrentPage - 1);
            return View(await _mediator.Send(query));
        }

        [HttpGet]
        public async Task<IActionResult> CustomerDetails(int id)
        {
            try
            {
                return View(await _mediator.Send(new GetCustomerDetailsQuery { CustomerId = id }));
            }
            catch (NotFoundException)
            {
                TempData["Error"] = $"Customer with ID {id} not found.";
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
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
                var success = await _mediator.Send(query);
                if (success)
                {
                    TempData["Message"] = "Successfully registred customer";
                    return RedirectToAction(nameof(Register));
                }

                TempData["Error"] = "An error occured while registrering the customer, try again shortly.";

            }
            return View(query);
        }


        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var customer = await _mediator.Send(new GetCustomerQuery { CustomerId = id });
                var command = new UpdateCustomerCommand
                {
                    ProfileData = customer
                };
                return View(command);
            }
            catch (NotFoundException)
            {
                return NotFound();
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
                    var customer = await _mediator.Send(command);
                    TempData["Message"] = "Successfully updated the customers profile.";
                    return RedirectToAction(nameof(Edit), new { id = customer.ProfileData.CustomerId });
                }
                catch (NotFoundException)
                {
                    return NotFound();
                }
            }
            return View(command);
        }
    }
}