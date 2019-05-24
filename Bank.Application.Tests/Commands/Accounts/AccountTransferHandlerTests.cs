using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Accounts.Commands.CreateAccountTransfer;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using Bank.Application.Tests.Infrastructure;
using Bank.Common;
using Shouldly;
using Xunit;

namespace Bank.Application.Tests.Commands.Accounts
{
    [Collection("Account Transfer Handler Tests")]
    public class AccountTransferHandlerTests
    {

        [Fact(DisplayName = "Transfer_ValidAmounts_CreditAndDebitCorrectly")]
        public async Task Transfer_ValidAmounts_CreditAndDebitCorrectly()
        {
            // Arrange
            IDateTime time = new MockDateTime();
            var context = FakeContext.Get();

            // Act
            var sut = new CreateAccountTransferCommandHandler(context, time);
            await sut.Handle(new CreateAccountTransferCommand { AccountIdFrom = 1, AccountIdTo = 2, Amount = 250m }, CancellationToken.None);

            // Assert
            var accOne = context.Accounts.Single(a => a.AccountId == 1);
            var accTwo = context.Accounts.Single(a => a.AccountId == 2);
            accOne.Balance.ShouldBe(250m);
            accTwo.Balance.ShouldBe(750m);
        }

        [Fact(DisplayName = "Transfer_ValidAmounts_CreateTransactions")]
        public async Task Transfer_ValidAmounts_CreateTransactions()
        {
            // Arrange
            IDateTime time = new MockDateTime();
            var context = FakeContext.Get();

            // Act
            var sut = new CreateAccountTransferCommandHandler(context, time);
            await sut.Handle(new CreateAccountTransferCommand { AccountIdFrom = 1, AccountIdTo = 2, Amount = 250m }, CancellationToken.None);

            // Assert
            var transactionFrom = context.Transactions.Single(t => t.AccountId == 1);
            var transactionTo = context.Transactions.Single(t => t.AccountId == 2);
            transactionFrom.Operation.ShouldBe(Operation.TransferDebit);
            transactionFrom.Type.ShouldBe(TransactionType.Debit);
            transactionFrom.Amount.ShouldBe(-250m);
            transactionFrom.Balance.ShouldBe(250m);
            transactionTo.Operation.ShouldBe(Operation.Transfer);
            transactionTo.Type.ShouldBe(TransactionType.Credit);
            transactionTo.Amount.ShouldBe(250m);
            transactionTo.Balance.ShouldBe(750m);
        }

        [Fact(DisplayName = "Transfer_MoreThanBalance_ThrowsException")]
        public async Task Transfer_MoreThanBalance_ThrowsException()
        {
            // Arrange
            IDateTime time = new MockDateTime();
            var context = FakeContext.Get();

            // Act
            var sut = new CreateAccountTransferCommandHandler(context, time);

            //Assert
            await Assert.ThrowsAsync<InsufficientFundsException>(() =>
                sut.Handle(new CreateAccountTransferCommand
                { AccountIdFrom = 1, AccountIdTo = 2, Amount = 700m }, CancellationToken.None));
            context.Accounts.Single(a => a.AccountId == 1).Balance.ShouldBe(500m);
        }

        [Fact(DisplayName = "Transfer_NegativeAmount_ThrowsException")]
        public async Task Transfer_NegativeAmount_ThrowsException()
        {
            // Arrange
            IDateTime time = new MockDateTime();
            var context = FakeContext.Get();

            // Act
            var sut = new CreateAccountTransferCommandHandler(context, time);

            //Assert
            await Assert.ThrowsAsync<NegativeAmountException>(() =>
                sut.Handle(new CreateAccountTransferCommand
                { AccountIdFrom = 1, AccountIdTo = 2, Amount = -200m }, CancellationToken.None));
            context.Accounts.Single(a => a.AccountId == 1).Balance.ShouldBe(500m);

        }
    }
}
