using System;
using System.Collections.Generic;

namespace Bank.Domain.Entities
{
    public partial class Account
    {
        public Account()
        {
            Dispositions = new HashSet<Disposition>();
            Transactions = new HashSet<Transaction>();
            Loans = new HashSet<Loan>();
            PermenentOrders = new HashSet<PermenentOrder>();
        }

        public int AccountId { get; set; }
        public string Frequency { get; set; }
        public DateTime Created { get; set; }
        public decimal? Balance { get; set; }

        public virtual ICollection<Disposition> Dispositions { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<PermenentOrder> PermenentOrders { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
    }
}
