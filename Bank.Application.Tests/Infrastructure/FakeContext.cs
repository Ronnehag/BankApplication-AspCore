using System;
using Bank.Domain.Entities;
using Bank.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Tests.Infrastructure
{
    public class FakeContext
    {
        /// <summary>
        /// Mocking the context using in memory database with 4 accounts.
        /// Accounts ID 1-5. ID 5 has a balance of 100 000 and 1-4 500
        /// The tests are depending on the mocked accounts.
        /// Don't change the balance directly in this method.
        /// </summary>
        /// <returns>BankAppDataContext</returns>
        public static BankAppDataContext Get()
        {
            var options = new DbContextOptionsBuilder<BankAppDataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new BankAppDataContext(options);
            var accounts = new[]
            {
                new Account {AccountId = 1, Balance = 500m, Frequency = "Monthly", Created = DateTime.Now},
                new Account {AccountId = 2, Balance = 500m, Frequency = "Monthly", Created = DateTime.Now},
                new Account {AccountId = 3, Balance = 500m, Frequency = "Monthly", Created = DateTime.Now},
                new Account {AccountId = 4, Balance = 500m, Frequency = "Monthly", Created = DateTime.Now},
                new Account {AccountId = 5, Balance = 100000m, Frequency = "Monthly", Created = DateTime.Now},
            };

            context.Accounts.AddRange(accounts);
            context.SaveChanges();
            return context;
        }
    }
}
