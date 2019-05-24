using System;

namespace Bank.Application.Transactions.Queries
{
    public class TransactionDto
    {
        public DateTime? Date { get; set; }
        public string Type { get; set; }
        public string Operation { get; set; }
        public string Amount { get; set; }
        public string Balance { get; set; }

        public string Value { get; set; }
    }
}
