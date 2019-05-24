using System;

namespace Bank.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"{name} ID {key} was not found.")
        {
        }
    }
}
