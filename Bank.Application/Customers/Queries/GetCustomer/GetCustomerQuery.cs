using MediatR;

namespace Bank.Application.Customers.Queries.GetCustomer
{
    public class GetCustomerQuery : IRequest<CustomerProfile>
    {
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
    }
}
