
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Exceptions;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Customers.Queries.GetCustomer
{
    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerProfile>
    {
        private readonly IBankDbContext _context;

        public GetCustomerQueryHandler(IBankDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerProfile> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            Customer customer;
            if (!string.IsNullOrEmpty(request.CustomerEmail))
            {
                customer = await _context.Customers.SingleOrDefaultAsync(c => c.Emailaddress == request.CustomerEmail, cancellationToken);
            }
            else
            {
                customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);
            }
            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), request.CustomerId);
            }

            var model = new CustomerProfile
            {
                CustomerId = customer.CustomerId,
                TelephoneNumber = customer.Telephonenumber,
                TelephoneCountryCode = customer.Telephonecountrycode,
                NationalId = customer.NationalId,
                Surname = customer.Surname,
                EmailAdress = customer.Emailaddress,
                GivenName = customer.Givenname,
                Birthday = customer.Birthday,
                Gender = customer.Gender,
                Address = new CustomerAddress
                {
                    CountryCode = customer.CountryCode,
                    Country = customer.Country,
                    City = customer.City,
                    StreetAdress = customer.Streetaddress,
                    ZipCode = customer.Zipcode
                }
            };

            return model;
        }
    }
}
