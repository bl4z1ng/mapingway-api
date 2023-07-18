using FluentValidation;
using Mapingway.Application.Abstractions.Validation;

namespace Mapingway.Application.Users.Commands.Register;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(
        IPasswordValidationRulesProvider passwordRulesProvider,
        IEmailValidationRulesProvider emailRulesProvider)
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .Must((c, _) => emailRulesProvider.IsValidEmail(c.Email))
                .WithMessage("You passed an invalid email.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(30)
            .Must((c, _) => passwordRulesProvider.HasNOrMoreLetters(c.Password))
                .WithMessage($"Password must have at least {passwordRulesProvider.NumberOfLetters} letters.");

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(256);
    }
}