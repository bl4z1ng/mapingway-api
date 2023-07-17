using FluentValidation;
using Mapingway.Application.Abstractions;

namespace Mapingway.Application.Users.Commands.Register;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IValidationRulesProvider rulesProvider)
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .Must((c, _) => rulesProvider.IsValidEmail(c.Email))
                .WithMessage("You passed an invalid email.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(30)
            .Must((c, _) => rulesProvider.HasNOrMoreLetters(c.Password))
                .WithMessage("Password must have at least 3 letters.");

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MinimumLength(2);
    }
}