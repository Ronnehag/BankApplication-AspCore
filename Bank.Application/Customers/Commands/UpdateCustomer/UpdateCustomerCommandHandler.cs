using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Exceptions;
using Bank.Application.Interfaces;
using Bank.Application.Models;
using Bank.Common.Extensions;
using Bank.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, IResult>
    {
        private readonly IBankDbContext _context;

        public UpdateCustomerCommandHandler(IBankDbContext context)
        {
            _context = context;
        }

        public async Task<IResult> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == request.Profile.CustomerId, cancellationToken);
            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), request.Profile.CustomerId);
            }

            customer.City = request.Profile.Address.City.ToUpperInvariant();
            customer.Country = request.Profile.Address.Country.ToFirstLetterUpper();
            customer.Streetaddress = request.Profile.Address.StreetAdress.ToFirstLetterUpper();
            customer.Zipcode = request.Profile.Address.ZipCode;
            customer.CountryCode = request.Profile.Address.CountryCode.ToUpperInvariant();
            customer.Gender = request.Profile.Gender;
            customer.Birthday = request.Profile.Birthday;
            customer.Surname = request.Profile.Surname.ToFirstLetterUpper();
            customer.Givenname = request.Profile.GivenName.ToFirstLetterUpper();
            customer.Emailaddress = request.Profile.EmailAdress.ToFirstLetterUpper();
            customer.Telephonenumber = request.Profile.TelephoneNumber;
            customer.Telephonecountrycode = request.Profile.TelephoneCountryCode;
            customer.NationalId = request.Profile.NationalId;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return new BaseResult { IsSuccess = true, Success = "Successfully updated the customers profile." };
        }
    }
}
