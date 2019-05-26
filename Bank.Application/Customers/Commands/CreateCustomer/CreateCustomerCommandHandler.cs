using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Enumerations;
using Bank.Application.Interfaces;
using Bank.Application.Models;
using Bank.Common;
using Bank.Common.Extensions;
using Bank.Domain.Entities;
using MediatR;

namespace Bank.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, IResult>
    {
        private readonly IBankDbContext _context;
        private readonly IDateTime _time;

        public CreateCustomerCommandHandler(IBankDbContext context, IDateTime time)
        {
            _context = context;
            _time = time;
        }

        public async Task<IResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Givenname = request.Profile.GivenName.ToFirstLetterUpper(),
                Surname = request.Profile.Surname.ToFirstLetterUpper(),
                Gender = request.Profile.Gender,
                City = request.Profile.Address.City.ToUpperInvariant(),
                Country = request.Profile.Address.Country.ToFirstLetterUpper(),
                Streetaddress = request.Profile.Address.StreetAdress.ToFirstLetterUpper(),
                Emailaddress = request.Profile.EmailAdress.ToFirstLetterUpper(),
                CountryCode = request.Profile.Address.CountryCode.ToUpperInvariant(),
                Zipcode = request.Profile.Address.ZipCode,
                Birthday = request.Profile.Birthday,
                NationalId = request.Profile.NationalId,
                Telephonecountrycode = request.Profile.TelephoneCountryCode,
                Telephonenumber = request.Profile.TelephoneNumber
            };

            var account = new Account
            {
                Balance = 0m,
                Created = _time.Now,
                Frequency = AccountFrequency.Monthly
            };

            _context.Customers.Add(customer);
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync(cancellationToken);

            var disposition = new Disposition
            {
                CustomerId = customer.CustomerId,
                AccountId = account.AccountId,
                Type = DispositionType.Owner
            };

            _context.Dispositions.Add(disposition);
            var result = await _context.SaveChangesAsync(cancellationToken);
            if (result == 1)
            {
                return new BaseResult { IsSuccess = true, Success = "Successfully registred customer" };
            }
            return new BaseResult { IsSuccess = false, Success = "An error occured while registrering the customer, try again shortly." };
        }
    }
}
