using System;
using Bank.Common;

namespace Bank.Infrastructure
{
    public class MachineDateTime : IDateTime
    {
        public DateTime Now { get; set; } = DateTime.Now;

        public int CurrentYear => DateTime.Now.Year;
        public int CurrentDay => DateTime.Now.Day;

        public override string ToString()
        {
            return Now.ToString("yyyy-MM-dd");
        }
    }
}
