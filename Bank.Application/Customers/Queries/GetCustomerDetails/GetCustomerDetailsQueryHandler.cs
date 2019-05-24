using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Accounts.Queries.GetCustomerAccounts;
using Bank.Application.Cards;
using Bank.Application.Cards.Queries;
using Bank.Application.Exceptions;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Customers.Queries.GetCustomerDetails
{
    public class GetCustomerDetailsQueryHandler : IRequestHandler<GetCustomerDetailsQuery, CustomerDto>
    {
        private readonly IBankDbContext _context;
        private readonly IMediator _mediator;

        public GetCustomerDetailsQueryHandler(IBankDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<CustomerDto> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
        {

            Customer customer;
            if (!string.IsNullOrEmpty(request.CustomerEmail))
            {
                customer = await _context.Customers
                    .SingleOrDefaultAsync(c => string.Equals(c.Emailaddress, request.CustomerEmail, StringComparison.CurrentCultureIgnoreCase), cancellationToken);
            }
            else
            {
                customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == request.CustomerId, cancellationToken);
            }
            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), request.CustomerId);
            }
            var model = new CustomerDto
            {
                GivenName = customer.Givenname,
                Surname = customer.Surname,
                CustomerId = customer.CustomerId,
                Birthday = customer.Birthday,
                EmailAdress = customer.Emailaddress,
                TelephoneCountryCode = customer.Telephonecountrycode,
                TelephoneNumber = customer.Telephonenumber,
                NationalId = customer.NationalId,
                Adress = new CustomerAdress
                {
                    Country = customer.Country,
                    City = customer.City,
                    CountryCode = customer.CountryCode,
                    StreetAdress = customer.Streetaddress,
                    ZipCode = customer.Zipcode
                },
                Accounts = await _mediator.Send(new GetCustomerAccountsQuery { CustomerId = customer.CustomerId }, cancellationToken),
                Cards = await _mediator.Send(new GetCustomerCardsQuery { CustomerId = customer.CustomerId }, cancellationToken)
            };
            return model;
        }

    }
}
