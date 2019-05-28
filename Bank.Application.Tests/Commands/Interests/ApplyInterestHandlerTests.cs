using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Enumerations;
using Bank.Application.Interests.Commands;
using Bank.Application.Tests.Infrastructure;
using Shouldly;
using Xunit;


namespace Bank.Application.Tests.Commands.Interests
{
    [Collection("Savings Interests")]
    public class ApplyInterestHandlerTests
    {
        /* Calculation Used, Example:
        * Balance: 100000
        * Interest: 5%
        * Days a year: 360
        * Timespan: 60 days
        *
        * (100000 * (5 / 100) / 360) * 60 + 100000
        * ~Rounded to 2 decimals.
        * Answer: 100833.33
        */

        [Fact(DisplayName = "Interest_ApplyInterest_CalculatedCorrectly")]
        public async Task Loan_ApplyInterest_CalculatedCorrectly()
        {
            // Arrange
            var context = FakeContext.Get();
            var mockDateTime = new MockDateTime { Now = DateTime.Today.AddDays(60).Date };
            var lastCalculatedDate = DateTime.Today.Date;
            var account = context.Accounts.Single(a => a.AccountId == 5);

            // Act
            var sut = new ApplyInterestCommandHandler(context, mockDateTime);
            await sut.Handle(new ApplyInterestsCommand { AccountId = 5, APR = 5, LastCalculatedDate = lastCalculatedDate }, CancellationToken.None);

            // Assert
            account.Balance.ShouldBe(100833.33m);
        }

        [Fact(DisplayName = "Interest_ApplyInterest_CreatesValidTransaction")]
        public async Task Interest_ApplyInterest_CreatesValidTransaction()
        {
            // Arrange
            var context = FakeContext.Get();
            var mockDateTime = new MockDateTime { Now = DateTime.Today.AddDays(600).Date };
            var lastCalculatedDate = DateTime.Today.Date;
            var account = context.Accounts.Single(a => a.AccountId == 5);

            // Act
            var sut = new ApplyInterestCommandHandler(context, mockDateTime);
            await sut.Handle(new ApplyInterestsCommand { AccountId = 5, APR = 5, LastCalculatedDate = lastCalculatedDate }, CancellationToken.None);

            // Assert
            account.Transactions.Count.ShouldBe(1);
            var transaction = context.Transactions.Single(t => t.AccountId == 5);
            transaction.Balance.ShouldBe(108333.33m);
            transaction.Amount.ShouldBe(8333.33m);
            transaction.AccountId.ShouldBe(5);
            transaction.Date.ShouldBe(mockDateTime.Now);
            transaction.Operation.ShouldBe(Operation.Credit);
            transaction.Type.ShouldBe(TransactionType.Credit);
            transaction.Symbol.ShouldBe(Interest.Symbol);
        }

        [Fact(DisplayName = "Interest_ZeroBalance_ShouldNotApplyInterest")]
        public async Task Interest_ZeroBalance_ShouldNotApplyInterest()
        {
            // Arrange
            var context = FakeContext.Get();
            var mockDateTime = new MockDateTime { Now = DateTime.Today.AddDays(200).Date };
            var calculatedDate = DateTime.Today.Date;
            var account = context.Accounts.Single(a => a.AccountId == 5);
            account.Balance = 0m;
            context.SaveChanges();

            // Act
            var sut = new ApplyInterestCommandHandler(context, mockDateTime);
            await sut.Handle(new ApplyInterestsCommand { AccountId = 5, APR = 5, LastCalculatedDate = calculatedDate }, CancellationToken.None);

            // Assert
            account.Balance.ShouldBe(0m);
            account.Transactions.Count.ShouldBe(0);
        }

    }
}
