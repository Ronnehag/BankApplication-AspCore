using System;
namespace Bank.Application.Exceptions
{
    public class NegativeAmountException : Exception
    {
        public NegativeAmountException(string name, object key)
            : base("Can't transfer a negative amount.")
        {
        }
    }
}
