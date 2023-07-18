using FluentValidation;
using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Validation;

namespace Mapingway.Application.Tokens.Commands.Revoke;

public class RevokeTokenValidator : AbstractValidator<RevokeTokenCommand>
{
    public RevokeTokenValidator(IEmailValidationRulesProvider emailRules)
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .Must((c, _) => emailRules.IsValidEmail(c.Email))
            .WithMessage("You passed an invalid email.");
    }
}