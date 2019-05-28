using FluentValidation;

namespace Bank.Application.Accounts.Commands.CreateAccountTransfer
{
    public class CreateAccountTransferCommandValidator : AbstractValidator<CreateAccountTransferCommand>
    {
        public CreateAccountTransferCommandValidator()
        {
            RuleFor(x => x.Amount)
                .InclusiveBetween(1.0m, 99999999999.99m).WithMessage("Can't enter a negative amount.")
                .ScalePrecision(2, 13).WithMessage("Maximum 13 digits and 2 decimals");
        }
    }
}
