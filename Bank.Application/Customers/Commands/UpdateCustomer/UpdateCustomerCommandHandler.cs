using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Exceptions;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerCommand>
    {
        private readonly IBankDbContext _context;

        public UpdateCustomerCommandHandler(IBankDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateCustomerCommand> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerId == request.ProfileData.CustomerId, cancellationToken);
            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), request.ProfileData.CustomerId);
            }

            customer.City = request.ProfileData.Adress.City.ToUpperInvariant();
            customer.Country = request.ProfileData.Adress.Country;
            customer.Streetaddress = request.ProfileData.Adress.StreetAdress;
            customer.Zipcode = request.ProfileData.Adress.ZipCode;
            customer.CountryCode = request.ProfileData.Adress.CountryCode;
            customer.Gender = request.ProfileData.Gender;
            customer.Birthday = request.ProfileData.Birthday;
            customer.Surname = request.ProfileData.Surname;
            customer.Givenname = request.ProfileData.GivenName;
            customer.Emailaddress = request.ProfileData.EmailAdress;
            customer.Telephonenumber = request.ProfileData.TelephoneNumber;
            customer.Telephonecountrycode = request.ProfileData.TelephoneCountryCode;
            customer.NationalId = request.ProfileData.NationalId;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync(cancellationToken);

            return request;
        }
    }
}
