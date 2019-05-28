using FluentValidation;

namespace Bank.Application.Accounts.Commands.CreateAccountDebit
{
    public class CreateAccountDebitCommandValidator : AbstractValidator<CreateAccountDebitCommand>
    {
        public CreateAccountDebitCommandValidator()
        {
            RuleFor(x => x.Amount)
                .ScalePrecision(2, 13).WithMessage("Maximum 13 digits and 2 decimals")
                .InclusiveBetween(1.00m, 99999999999.99m).WithMessage("Can't enter a negative amount.");
        }
    }
}
