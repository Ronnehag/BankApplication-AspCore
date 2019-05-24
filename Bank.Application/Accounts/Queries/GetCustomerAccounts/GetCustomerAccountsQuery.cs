using System.Collections.Generic;
using MediatR;

namespace Bank.Application.Accounts.Queries.GetCustomerAccounts
{
    public class GetCustomerAccountsQuery : IRequest<IEnumerable<CustomerAccountDto>>
    {
        public int CustomerId { get; set; }
    }
}
