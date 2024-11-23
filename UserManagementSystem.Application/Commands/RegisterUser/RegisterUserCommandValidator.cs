using FluentValidation;

namespace UserManagementSystem.Application.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Password must be at least 3 characters long.")
                .MaximumLength(15).WithMessage("Username must be less than 15 characters.");

            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required.");

            RuleFor(x => x.Password).NotEmpty()
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(20).WithMessage("Password must be less than 20 characters.");
        }
    }
}
