using System.Collections.Generic;

namespace Bank.Application.Customers.Queries.GetCustomerList
{
    public class CustomerListViewModel
    {
        public int Total { get; set; }
        public int NumberOfPages { get; set; }
        public bool HasMorePages { get; set; }
        public bool HasPreviousPages { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int CurrentPage { get; set; } = 1;

        


        public IEnumerable<CustomerListDto> Customers { get; set; }
    }
}
