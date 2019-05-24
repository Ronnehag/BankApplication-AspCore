using Bank.Application.Customers.Queries.GetCustomer;
using MediatR;

namespace Bank.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<UpdateCustomerCommand>
    {
        public CustomerProfile ProfileData { get; set; }
    }
}
