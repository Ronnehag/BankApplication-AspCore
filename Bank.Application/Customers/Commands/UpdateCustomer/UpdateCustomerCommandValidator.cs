using FluentValidation;

namespace Bank.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Profile.CustomerId).NotEmpty();

            RuleFor(x => x.Profile.GivenName)
                .MaximumLength(100).WithMessage("Maximum length is 100 characters")
                .NotEmpty().WithMessage("Name is required")
                .Matches(@"^[\p{L}]+$").WithMessage("Numbers or symbols now allowed");

            RuleFor(x => x.Profile.Surname)
                .MaximumLength(100).WithMessage("Maximum length is 100 characters")
                .NotEmpty().WithMessage("Name is required")
                .Matches(@"^[\p{L}]+$").WithMessage("Numbers or symbols now allowed");

            RuleFor(x => x.Profile.NationalId)
                .MaximumLength(20).WithMessage("Maximum length is 20 characters");

            RuleFor(x => x.Profile.TelephoneNumber)
                .MaximumLength(25).WithMessage("Maximum length is 25 characters");

            RuleFor(x => x.Profile.TelephoneCountryCode)
                .MaximumLength(10).WithMessage("Maximum length is 10 characters");

            RuleFor(x => x.Profile.EmailAdress)
                .MaximumLength(100).WithMessage("Maximum length is 100 characters")
                .EmailAddress().WithMessage("A valid email address is required");

            RuleFor(x => x.Profile.Gender)
                .MaximumLength(6).WithMessage("Maximum length is 6 characters")
                .NotEmpty().WithMessage("Select your gender");

            RuleFor(x => x.Profile.Address.StreetAdress)
                .MaximumLength(100).WithMessage("Maximum length is 100 characters")
                .NotEmpty().WithMessage("Street address is required");

            RuleFor(x => x.Profile.Address.ZipCode)
                .MaximumLength(15).WithMessage("Maximum length is 15 characters")
                .NotEmpty().WithMessage("Zip code is required");

            RuleFor(x => x.Profile.Address.City)
                .MaximumLength(100).WithMessage("Maximum length is 100 characters")
                .NotEmpty().WithMessage("City is required")
                .Matches(@"^[\p{L}]+$").WithMessage("Numbers or symbols now allowed");

            RuleFor(x => x.Profile.Address.Country)
                .MaximumLength(100).WithMessage("Maximum length is 100 characters")
                .NotEmpty().WithMessage("City is required")
                .Matches(@"^[\p{L}]+$").WithMessage("Numbers or symbols now allowed");

            RuleFor(x => x.Profile.Address.CountryCode)
                .MaximumLength(2).WithMessage("Maximum length is 2 characters")
                .NotEmpty().WithMessage("Country code is required")
                .Matches(@"^[\p{L}]+$").WithMessage("Numbers or symbols now allowed");
        }
    }
}
