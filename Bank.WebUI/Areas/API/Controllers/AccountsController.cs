using System.Threading.Tasks;
using Bank.Application.Transactions.Queries.GetAccountTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebUI.Areas.API.Controllers
{
    [ApiController]
    public class AccountsController : BaseApiController
    {
        // GET: api/accounts/id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetAccountTransactionsQuery { AccountId = id }));
        }

        // GET: api/accounts/id/limit=int/offset=int
        [HttpGet("{id:int}/limit={limit:int}/offset={offset:int}")]
        public async Task<IActionResult> Get(int id, int offset, int limit)
        {
            return Ok(await Mediator.Send(new GetAccountTransactionsQuery { AccountId = id, Limit = limit, Offset = offset }));
        }


    }
}