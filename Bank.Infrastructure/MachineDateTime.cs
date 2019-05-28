using System;
using Bank.Common;

namespace Bank.Infrastructure
{
    public class MachineDateTime : IDateTime
    {
        public DateTime Now { get; } = DateTime.Now;

        public override string ToString()
        {
            return Now.ToString("yyyy-MM-dd");
        }
    }
}
