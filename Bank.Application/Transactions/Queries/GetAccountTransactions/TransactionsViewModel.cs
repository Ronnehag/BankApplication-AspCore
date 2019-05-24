using System.Collections.Generic;

namespace Bank.Application.Transactions.Queries.GetAccountTransactions
{
    public class TransactionsViewModel
    {
        public int Total { get; set; }
        public int NumberOfPages { get; set; }
        public bool HasMorePages { get; set; }


        public IEnumerable<TransactionDto> Transactions { get; set; }
    }
}
