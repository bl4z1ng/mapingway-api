using FluentValidation;
using Mapingway.Application.Contracts.Abstractions.Validation;

namespace Mapingway.Application.Auth.Commands.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator(IEmailValidationRulesProvider emailRules)
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .Must((c, _) => emailRules.IsEmailValid(c.Email))
            .WithMessage("You passed an invalid email.");
    }
}