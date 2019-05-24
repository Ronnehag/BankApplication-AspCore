using MediatR;

namespace Bank.Application.Customers.Queries.GetCustomerDetails
{
    public class GetCustomerDetailsQuery : IRequest<CustomerDto>
    {
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
    }
}
