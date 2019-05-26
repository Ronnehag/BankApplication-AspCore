using Bank.Application.Customers.Queries.GetCustomer;
using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<IResult>
    {
        public CustomerProfile Profile { get; set; }
    }
}
