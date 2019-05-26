using System.Security.Claims;
using System.Threading.Tasks;
using Bank.Application.Customers.Queries.GetCustomerDetails;
using Bank.Application.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebUI.Areas.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomersController : BaseApiController
    {
        // GET: /api/me
        [HttpGet("/api/me")]
        public async Task<IActionResult> MyProfile()
        {
            if (User.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                var email = User.Identity.Name;
                try
                {
                    var userDto = await Mediator.Send(new GetCustomerDetailsQuery { CustomerEmail = email });
                    return Ok(userDto);
                }
                catch (NotFoundException)
                {
                    return NotFound("Customer not found");
                }
            }
            return BadRequest("Error occured while getting your data, email not found. Have you typed it correctly?");
        }
    }
}