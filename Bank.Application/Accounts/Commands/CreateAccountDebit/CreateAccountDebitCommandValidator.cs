using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Bank.Application.Accounts.Commands.CreateAccountDebit
{
    public class CreateAccountDebitCommandValidator : AbstractValidator<CreateAccountDebitCommand>
    {
        public CreateAccountDebitCommandValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty().WithMessage("Account number required");
            RuleFor(x => x.Amount).InclusiveBetween(1.0m, 99999999999.99m).WithMessage("Can't enter a negative amount.");
        }
    }
}
