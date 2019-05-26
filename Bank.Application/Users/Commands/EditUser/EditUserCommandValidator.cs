using FluentValidation;

namespace Bank.Application.Users.Commands.EditUser
{
    public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
    {
        public EditUserCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty();

            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.NewEmail)
                .MaximumLength(100).WithMessage("Max length 100 characters").When(s => !string.IsNullOrWhiteSpace(s.NewEmail));

            RuleFor(x => x.Password)
                .MaximumLength(30).WithMessage("Max length is 30 characters").When(s => !string.IsNullOrWhiteSpace(s.Password)); ;
        }
    }
}
