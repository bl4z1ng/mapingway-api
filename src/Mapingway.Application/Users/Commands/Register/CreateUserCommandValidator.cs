using FluentValidation;
using Mapingway.Application.Abstractions.Validation;

namespace Mapingway.Application.Users.Commands.Register;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(
        IPasswordValidationRulesProvider passwordRules,
        IEmailValidationRulesProvider emailRules)
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .Must((c, _) => emailRules.IsEmailValid(c.Email))
                .WithMessage("Email is invalid.")
            .MustAsync((email, _) => emailRules.IsEmailUnique(email))
                .WithMessage("Email is already taken.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(30)
            .Must((c, _) => passwordRules.HasNOrMoreLetters(c.Password))
                .WithMessage($"Password must have at least {passwordRules.NumberOfLetters} letters.");

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(256);
    }
}