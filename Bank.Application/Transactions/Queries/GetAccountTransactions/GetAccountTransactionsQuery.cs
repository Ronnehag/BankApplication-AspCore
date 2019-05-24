using MediatR;

namespace Bank.Application.Transactions.Queries.GetAccountTransactions
{
    public class GetAccountTransactionsQuery : IRequest<TransactionsViewModel>
    {
        private int _limit = 20;
        public int AccountId { get; set; }
        public int Offset { get; set; } = 0;

        public int Limit
        {
            get => _limit;
            set => _limit = value > 20 || value == 0 ? _limit : value;
        }
    }
}
