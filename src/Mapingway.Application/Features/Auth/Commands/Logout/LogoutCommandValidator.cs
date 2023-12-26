using FluentValidation;
using Mapingway.Application.Contracts.Validation;

namespace Mapingway.Application.Features.Auth.Commands.Logout;

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