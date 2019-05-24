using System.Threading.Tasks;
using Bank.Application.Transactions.Queries.GetAccountTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.WebUI.Areas.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/accounts/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _mediator.Send(new GetAccountTransactionsQuery { AccountId = id }));
        }

        // GET: api/accounts/id/limit=int/offset=int
        [HttpGet("{id}/limit={limit}/offset={offset}")]
        public async Task<IActionResult> Get(int id, int offset, int limit)
        {
            return Ok(await _mediator.Send(new GetAccountTransactionsQuery { AccountId = id, Limit = limit, Offset = offset }));
        }
    }
}