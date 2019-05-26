using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Customers.Queries.GetCustomerList
{
    public class GetCustomerListQueryHandler : IRequestHandler<GetCustomerListQuery, CustomerListViewModel>
    {
        private readonly IBankDbContext _context;

        public GetCustomerListQueryHandler(IBankDbContext context)
        {
            _context = context;
        }

        public async Task<CustomerListViewModel> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Customers.AsQueryable().AsNoTracking();
            var nameExists = !string.IsNullOrWhiteSpace(request.Name);
            var cityExists = !string.IsNullOrWhiteSpace(request.City);

            if (nameExists && !cityExists)
            {
                var names = request.Name.Split(" ");
                var firstName = names[0];
                if (names.Length > 1)
                {
                    var surName = names[1];
                    query = query
                        .Where(c => c.Givenname.StartsWith(firstName, StringComparison.OrdinalIgnoreCase) && c.Surname.StartsWith(surName))
                        .OrderByDescending(c => c.Givenname.Equals(firstName, StringComparison.OrdinalIgnoreCase))
                        .ThenByDescending(c =>c.Surname.Equals(surName, StringComparison.OrdinalIgnoreCase))
                        .AsNoTracking();
                }
                else
                {
                    query = query
                        .Where(c => c.Givenname.StartsWith(firstName, StringComparison.OrdinalIgnoreCase) ||
                                    c.Surname.StartsWith(firstName, StringComparison.OrdinalIgnoreCase))
                        .OrderByDescending(c => c.Givenname.Equals(firstName, StringComparison.OrdinalIgnoreCase))
                        .ThenByDescending(c => c.Givenname.StartsWith(firstName, StringComparison.OrdinalIgnoreCase))
                        .ThenByDescending(c => c.Surname.StartsWith(firstName)).AsNoTracking();
                }
            }
            else if (cityExists && !nameExists)
            {
                query = query.Where(c => c.City
                        .StartsWith(request.City, StringComparison.OrdinalIgnoreCase))
                        .OrderByDescending(c => c.City.Equals(request.City, StringComparison.OrdinalIgnoreCase))
                    .ThenByDescending(c => c.City.StartsWith(request.City, StringComparison.OrdinalIgnoreCase))
                    .AsNoTracking();

            }
            else if(cityExists && nameExists)
            {
                var names = request.Name.Split(" ");
                var firstName = names[0];
                if (names.Length > 1)
                {
                    var surName = names[1];
                    query = query.Where(c =>
                            (c.Givenname.StartsWith(firstName, StringComparison.OrdinalIgnoreCase) &&
                             c.Surname.StartsWith(surName, StringComparison.OrdinalIgnoreCase)) &&
                             c.City.StartsWith(request.City, StringComparison.OrdinalIgnoreCase))
                        .OrderByDescending(c => c.Givenname.StartsWith(request.Name, StringComparison.OrdinalIgnoreCase))
                        .ThenByDescending(c => c.Surname.StartsWith(request.Name, StringComparison.OrdinalIgnoreCase))
                        .ThenByDescending(c => c.City.StartsWith(request.City, StringComparison.OrdinalIgnoreCase)).AsNoTracking();

                }
                else
                {
                    query = query.Where(c =>
                            (c.Givenname.StartsWith(request.Name, StringComparison.OrdinalIgnoreCase) ||
                             c.Surname.StartsWith(request.Name, StringComparison.OrdinalIgnoreCase)) &&
                             c.City.StartsWith(request.City, StringComparison.OrdinalIgnoreCase))
                        .OrderByDescending(c => c.Givenname.StartsWith(request.Name, StringComparison.OrdinalIgnoreCase))
                        .ThenByDescending(c => c.Surname.StartsWith(request.Name, StringComparison.OrdinalIgnoreCase))
                        .ThenByDescending(c => c.City.StartsWith(request.City, StringComparison.OrdinalIgnoreCase)).AsNoTracking();
                }
            }
            else
            {
                query = query.OrderBy(c => c.CustomerId).AsNoTracking();
            }

            var totalCount = query.Count();
            return new CustomerListViewModel
            {
                Customers = await query.Skip(request.Offset).Take(request.Limit).Select(c => new CustomerListDto
                {
                    CustomerId = c.CustomerId,
                    Surname = c.Surname,
                    GivenName = c.Givenname,
                    NationalId = c.NationalId,
                    Address = new CustomerAddress
                    {
                        City = c.City,
                        StreetAdress = c.Streetaddress,
                        ZipCode = c.Zipcode,
                        Country = c.Country
                    }
                }).AsNoTracking().ToArrayAsync(cancellationToken),
                Total = totalCount,
                NumberOfPages = totalCount / request.Limit + 1,
                HasMorePages = request.Offset + request.Limit < totalCount,
                Name = request.Name,
                City = request.City,
                CurrentPage = request.CurrentPage,
                HasPreviousPages = request.Offset > 0
            };
        }
    }
}
