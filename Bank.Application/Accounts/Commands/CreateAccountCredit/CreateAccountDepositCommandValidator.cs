using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Bank.Application.Accounts.Commands.CreateAccountCredit
{
    public class CreateAccountDepositCommandValidator : AbstractValidator<CreateAccountDepositCommand>
    {
        public CreateAccountDepositCommandValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty().WithMessage("Account number required");
            RuleFor(x => x.Amount).InclusiveBetween(1.0m, 99999999999.99m).WithMessage("Can't enter a negative amount.");
        }
    }
}
