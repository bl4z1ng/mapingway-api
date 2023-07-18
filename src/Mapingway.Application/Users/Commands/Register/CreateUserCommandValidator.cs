using FluentValidation;
using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Validation;

namespace Mapingway.Application.Users.Commands.Register;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IPasswordValidationRules _validationRules;

    public CreateUserCommandValidator(
        IValidationRulesProvider rulesProvider,
        IPasswordValidationRules validationRules)
    {
        _validationRules = validationRules;
        RuleFor(c => c.Email)
            .NotEmpty()
            .Must((c, _) => rulesProvider.IsValidEmail(c.Email))
                .WithMessage("You passed an invalid email.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(30)
            .Must((c, _) => rulesProvider.HasNOrMoreLetters(c.Password))
                .WithMessage($"Password must have at least {rulesProvider.PasswordNumberOfLetters} letters.");

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(256);
    }
}