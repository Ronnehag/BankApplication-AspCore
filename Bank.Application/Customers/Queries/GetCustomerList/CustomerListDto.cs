using System;

namespace Bank.Application.Customers.Queries.GetCustomerList
{
    public class CustomerListDto
    {
        public int CustomerId { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public CustomerAdress Address { get; set; }
        public string NationalId { get; set; }

        public string GetFullName => GivenName + " " + Surname;
    }
}
