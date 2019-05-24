using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Enumerations;
using Bank.Application.Interests.Commands;
using Bank.Application.Tests.Infrastructure;
using Shouldly;
using Xunit;

namespace Bank.Application.Tests.Commands.Loans
{
    [Collection("Loan Interests")]
    public class ApplyInterestHandlerTests
    {
        [Fact(DisplayName = "Interest_ApplyInterests_CalculatedCorrectly")]
        public async Task Loan_ApplyInterests_CalculatedCorrectly()
        {
            // Arrange
            var context = FakeContext.Get();
            var time = new MockDateTime { Now = DateTime.Today.AddDays(60).Date };
            var lastCalculated = DateTime.Today.Date;

            // Act
            var sut = new ApplyInterestCommandHandler(context, time);
            await sut.Handle(new ApplyInterestsCommand { AccountId = 5, APR = Interest.Rate, LastCalculatedDate = lastCalculated }, CancellationToken.None);

            // Assert
            var account = context.Accounts.Single(a => a.AccountId == 5);
            account.Balance.ShouldBe(100833.33m);
        }

        [Fact(DisplayName = "Interest_ApplyInterest_CreateValidTransaction")]
        public async Task Interest_ApplyInterest_CreateValidTransaction()
        {
            // Arrange
            var context = FakeContext.Get();
            var time = new MockDateTime { Now = DateTime.Today.AddDays(200).Date };
            var lastCalculated = DateTime.Today.Date;

            // Act
            var sut = new ApplyInterestCommandHandler(context, time);
            await sut.Handle(new ApplyInterestsCommand { AccountId = 5, APR = Interest.Rate, LastCalculatedDate = lastCalculated }, CancellationToken.None);

            // Assert
            var account = context.Accounts.Single(a => a.AccountId == 5);
            account.Transactions.Count.ShouldBe(1);
            var transaction = context.Transactions.Single(t => t.AccountId == 5);
            transaction.Balance.ShouldBe(102777.78m);
            transaction.Amount.ShouldBe(2777.78m);
            transaction.AccountId.ShouldBe(5);
            transaction.Date.ShouldBe(time.Now);
            transaction.Operation.ShouldBe(Operation.Credit);
            transaction.Type.ShouldBe(TransactionType.Credit);
        }

    }
}
