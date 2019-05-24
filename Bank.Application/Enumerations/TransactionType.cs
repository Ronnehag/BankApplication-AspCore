namespace Bank.Application.Enumerations
{
    public static class TransactionType
    {
        public const string Debit = "Debit";
        public const string Credit = "Credit";
    }

    public static class Operation
    {
        public const string Withdrawal = "Withdrawal in Cash";
        public const string Credit = "Credit in Cash";
        public const string TransferDebit = "Remittance to Another Bank";
        public const string Transfer = "Collection from Another Bank";
    }
}
