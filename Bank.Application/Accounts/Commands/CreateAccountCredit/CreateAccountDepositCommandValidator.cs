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
            RuleFor(x => x.Amount).InclusiveBetween(1.00m, 99999999999.99m).WithMessage("Can't enter a negative amount.")
                .ScalePrecision(2, 13).WithMessage("Maximum 13 digits and 2 decimals");
        }
    }
}
