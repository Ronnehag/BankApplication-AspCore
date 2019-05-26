using FluentValidation;

namespace Bank.Application.Accounts.Commands.CreateAccountTransfer
{
    public class CreateAccountTransferCommandValidator : AbstractValidator<CreateAccountTransferCommand>
    {
        public CreateAccountTransferCommandValidator()
        {
            RuleFor(x => x.AccountIdFrom).NotEmpty().WithMessage("Account number required");
            RuleFor(x => x.AccountIdTo).NotEmpty().WithMessage("Account number required");
            RuleFor(x => x.Amount).InclusiveBetween(1.0m, 99999999999.99m).WithMessage("Can't enter a negative amount.");
        }
    }
}
