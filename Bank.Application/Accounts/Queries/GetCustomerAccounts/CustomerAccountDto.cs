using System.Globalization;

namespace Bank.Application.Accounts.Queries.GetCustomerAccounts
{
    public class CustomerAccountDto
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }

        public string PrintBalance()
        {
            return Balance.ToString("C", new CultureInfo("sv-SE"));
        }
    }
}
