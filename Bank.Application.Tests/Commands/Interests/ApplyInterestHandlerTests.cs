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
        * Balance: 10000
        * Interest: 5%
        * Days a year: 360
        * Timespan: 60 days
        *
        * (10000 * (5 / 100) / 360) * 60 + 10000
        * Rounded to 2 decimals.
        * Answer: 10083.33
        */

        [Fact(DisplayName = "Interest_ApplyInterest_CalculatedCorrectly")]
        public async Task Loan_ApplyInterest_CalculatedCorrectly()
        {
            // Arrange
            var context = FakeContext.Get();
            var mockDateTime = new MockDateTime { Now = DateTime.Today.AddDays(60).Date };
            var lastCalculatedDate = DateTime.Today.Date;
            var account = context.Accounts.Single(a => a.AccountId == 5);
            var endValue = Math.Round((account.Balance.Value * (Interest.Rate / 100) / Interest.DaysAYear) *
                                              (mockDateTime.Now - lastCalculatedDate).Days, 2) + account.Balance.Value;
            // Act
            var sut = new ApplyInterestCommandHandler(context, mockDateTime);
            await sut.Handle(new ApplyInterestsCommand { AccountId = 5, APR = Interest.Rate, LastCalculatedDate = lastCalculatedDate }, CancellationToken.None);

            // Assert
            account.Balance.ShouldBe(endValue);
        }

        [Fact(DisplayName = "Interest_ApplyInterest_CreatesValidTransaction")]
        public async Task Interest_ApplyInterest_CreatesValidTransaction()
        {
            // Arrange
            var context = FakeContext.Get();
            var mockDateTime = new MockDateTime { Now = DateTime.Today.AddDays(600).Date };
            var lastCalculatedDate = DateTime.Today.Date;
            var account = context.Accounts.Single(a => a.AccountId == 5);
            var transactionValue = Math.Round((account.Balance.Value * (Interest.Rate / 100) / Interest.DaysAYear) *
                                      (mockDateTime.Now - lastCalculatedDate).Days, 2);
            var endValue = transactionValue + account.Balance.Value;

            // Act
            var sut = new ApplyInterestCommandHandler(context, mockDateTime);
            await sut.Handle(new ApplyInterestsCommand { AccountId = 5, APR = Interest.Rate, LastCalculatedDate = lastCalculatedDate }, CancellationToken.None);

            // Assert
            account.Transactions.Count.ShouldBe(1);
            var transaction = context.Transactions.Single(t => t.AccountId == 5);
            transaction.Balance.ShouldBe(endValue);
            transaction.Amount.ShouldBe(transactionValue);
            transaction.AccountId.ShouldBe(5);
            transaction.Date.ShouldBe(mockDateTime.Now);
            transaction.Operation.ShouldBe(Operation.Credit);
            transaction.Type.ShouldBe(TransactionType.Credit);
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
            await sut.Handle(new ApplyInterestsCommand { AccountId = 5, APR = Interest.Rate, LastCalculatedDate = calculatedDate }, CancellationToken.None);

            // Assert
            account.Balance.ShouldBe(0m);
            account.Transactions.Count.ShouldBe(0);
        }

    }
}
