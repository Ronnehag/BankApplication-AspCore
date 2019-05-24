using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Enumerations;
using Bank.Application.Interfaces;
using Bank.Common;
using Bank.Domain.Entities;
using MediatR;

namespace Bank.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, bool>
    {
        private readonly IBankDbContext _context;
        private readonly IDateTime _time;

        public CreateCustomerCommandHandler(IBankDbContext context, IDateTime time)
        {
            _context = context;
            _time = time;
        }

        public async Task<bool> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Givenname = request.GivenName,
                Surname = request.Surname,
                Gender = request.Gender,
                City = request.City.ToUpperInvariant(),
                Country = request.Country,
                Streetaddress = request.Streetadress,
                Emailaddress = request.EmailAdress,
                CountryCode = request.CountryCode,
                Zipcode = request.Zipcode,
                Birthday = request.Birthday,
                NationalId = request.NationalId,
                Telephonecountrycode = request.TelephoneCountryCode,
                Telephonenumber = request.TelephoneNumber
            };

            var account = new Account
            {
                Balance = 0,
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
            return result == 1;
        }
    }
}
