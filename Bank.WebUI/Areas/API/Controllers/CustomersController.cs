using System.Security.Claims;
using System.Threading.Tasks;
using Bank.Application.Customers.Queries.GetCustomerDetails;
using Bank.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebUI.Areas.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/me
        [HttpGet("api/me")]
        public async Task<IActionResult> Get()
        {
            if (User.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                var email = User.Identity.Name;
                try
                {
                    var userDto = await _mediator.Send(new GetCustomerDetailsQuery { CustomerEmail = email });
                    return Ok(userDto);
                }
                catch (NotFoundException)
                {
                    return NotFound("Customer not found");
                }
            }

            return BadRequest("Error occured while getting your data, are you signed in with correct email?");
        }
    }
}