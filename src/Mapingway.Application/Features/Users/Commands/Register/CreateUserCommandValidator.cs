using FluentValidation;
using Mapingway.Application.Contracts.Abstractions.Validation;

namespace Mapingway.Application.Users.Commands.Register;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(
        INameValidationRulesProvider nameRules,
        IEmailValidationRulesProvider emailRules,
        IPasswordValidationRulesProvider passwordRules)
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MaximumLength(nameRules.MaxLength);
        
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
    }
}