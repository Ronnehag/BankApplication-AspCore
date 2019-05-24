using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Accounts.Commands.CreateAccountCredit;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using Bank.Application.Tests.Infrastructure;
using Bank.Common;
using Shouldly;
using Xunit;

namespace Bank.Application.Tests.Commands.Accounts
{
    [Collection("CommandCollection")]
    public class AccountCreditHandlerTests
    {
        [Fact(DisplayName = "Deposit_ValidDeposit_Credited")]
        public async Task Deposit_ValidDeposit_Credited()
        {
            // Arrange
            IDateTime time = new MockDateTime();
            var context = FakeContext.Get();

            // Act
            var sut = new CreateAccountDepositCommandHandler(context, time);
            await sut.Handle(new CreateAccountDepositCommand() { AccountId = 1, Amount = 500m }, CancellationToken.None);

            // Assert
            context.Accounts.Single(a => a.AccountId == 1).Balance.ShouldBe(1000m);
        }


        [Fact(DisplayName = "Deposit_Transaction_Created")]
        public async Task Deposit_Transaction_Created()
        {
            // Arrange
            IDateTime time = new MockDateTime();
            var context = FakeContext.Get();

            // Act
            var sut = new CreateAccountDepositCommandHandler(context, time);
            await sut.Handle(new CreateAccountDepositCommand() { AccountId = 1, Amount = 500m }, CancellationToken.None);

            // Assert
            var transaction = context.Transactions.Single(t => t.AccountId == 1);
            transaction.AccountId.ShouldBe(1);
            transaction.Amount.ShouldBe(500m);
            transaction.Balance.ShouldBe(1000m);
            transaction.Type.ShouldBe(TransactionType.Credit);
            transaction.Operation.ShouldBe(Operation.Credit);
        }


        [Fact(DisplayName = "Deposit_NegativeAmount_ThrowsException")]
        public async Task Deposit_NegativeAmount_ThrowsException()
        {
            // Arrange
            IDateTime time = new MockDateTime();
            var context = FakeContext.Get();

            // Arrange
            var sut = new CreateAccountDepositCommandHandler(context, time);

            // Assert
            await Assert.ThrowsAsync<NegativeAmountException>
                (() => sut.Handle(new CreateAccountDepositCommand() { AccountId = 1, Amount = -500m }, CancellationToken.None));
            context.Accounts.Single(a => a.AccountId == 1).Balance.ShouldBe(500m);
        }
    }
}

