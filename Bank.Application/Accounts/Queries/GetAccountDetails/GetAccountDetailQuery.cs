using MediatR;

namespace Bank.Application.Accounts.Queries.GetAccountDetails
{
    public class GetAccountDetailQuery : IRequest<AccountDetailDto>
    {
        public int AccountId { get; set; }
    }
}
