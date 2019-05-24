using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bank.Application.Accounts.Commands.CreateAccountDebit;
using Bank.Application.Enumerations;
using Bank.Application.Exceptions;
using Bank.Application.Tests.Infrastructure;
using Bank.Common;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Bank.Application.Tests.Commands.Accounts
{
    [Collection("Account Debit Handler Tests")]
    public class AccountDebitHandlerTests
    {
        [Fact(DisplayName = "Withdrawal_ValidWithdrawal_Debited")]
        public async Task Withdrawal_ValidWithdrawal_Debited()
        {
            // Arrange
            IDateTime time = new MockDateTime();
            var context = FakeContext.Get();

            // Act
            var sut = new CreateAccountDebitCommandHandler(context, time);
            await sut.Handle(new CreateAccountDebitCommand { AccountId = 4, Amount = 250.145612m }, CancellationToken.None);

            // Assert
            var account = await context.Accounts.Include(a => a.Transactions).SingleOrDefaultAsync(a => a.AccountId == 4);
            account.Balance.ShouldBe(249.85m);
        }

        [Fact(DisplayName = "Withdrawal_Transaction_Created")]
        public async Task Withdrawal_Transaction_Created()
        {
            // Arrange
            IDateTime time = new MockDateTime();
            var context = FakeContext.Get();

            // Act
            var sut = new CreateAccountDebitCommandHandler(context, time);
            await sut.Handle(new CreateAccountDebitCommand { AccountId = 3, Amount = 250m }, CancellationToken.None);

            // Assert
            var transaction = await context.Transactions.SingleOrDefaultAsync(t => t.AccountId == 3);
            transaction.Amount.ShouldBe(-250m);
            transaction.AccountId.ShouldBe(3);
            transaction.Operation.ShouldBe(Operation.Withdrawal);
            transaction.Type.ShouldBe(TransactionType.Debit);
        }

        [Fact(DisplayName = "Withdrawal_MoreThanBalance_ThrowsException")]
        public async Task Withdrawal_MoreThanBalance_ThrowsException()
        {
            // Arrange
            IDateTime time = new MockDateTime();
            var context = FakeContext.Get();

            // Act
            var sut = new CreateAccountDebitCommandHandler(context, time);

            // Assert
            await Assert.ThrowsAsync<InsufficientFundsException>(() =>
                sut.Handle(new CreateAccountDebitCommand { AccountId = 1, Amount = 501m }, CancellationToken.None));
            context.Accounts.Single(a => a.AccountId == 1).Balance.ShouldBe(500m);
        }

        [Fact(DisplayName = "Withdrawal_NegativeAmount_ThrowsException")]
        public async Task Withdrawal_NegativeAmount_ThrowsException()
        {
            // Arrange
            IDateTime time = new MockDateTime();
            var context = FakeContext.Get();

            // Arrange
            var sut = new CreateAccountDebitCommandHandler(context, time);

            // Assert
            await Assert.ThrowsAsync<NegativeAmountException>
                (() => sut.Handle(new CreateAccountDebitCommand() { AccountId = 1, Amount = -500m }, CancellationToken.None));
            context.Accounts.Single(a => a.AccountId == 1).Balance.ShouldBe(500m);
        }
    }
}
