using System;
using Bank.Common;

namespace Bank.Application.Tests.Infrastructure
{
    internal class MockDateTime : IDateTime
    {
        public DateTime Now { get; set; }
    }
}
