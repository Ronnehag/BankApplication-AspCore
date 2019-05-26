using Bank.Application.Customers.Queries.GetCustomer;
using Bank.Application.Interfaces;
using MediatR;

namespace Bank.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<IResult>
    {
        public CustomerProfile Profile { get; set; }
    }
}
