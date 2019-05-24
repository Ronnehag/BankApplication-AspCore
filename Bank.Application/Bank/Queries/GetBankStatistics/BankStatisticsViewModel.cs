using System.Globalization;

namespace Bank.Application.Bank.Queries.GetBankStatistics
{
    public class BankStatisticsViewModel
    {
        public int TotalCustomers { get; set; }
        public int TotalAccounts { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
