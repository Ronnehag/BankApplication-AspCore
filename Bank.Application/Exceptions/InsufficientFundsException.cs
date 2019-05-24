using System;

namespace Bank.Application.Exceptions
{
    public class InsufficientFundsException : Exception
    {

        public InsufficientFundsException(string name, object key)
            : base($"{name} - ID: ({key}) has insufficient funds.")
        {
        }
    }
}
