using FluentValidation;
using Mapingway.Application.Abstractions;

namespace Mapingway.Application.Tokens.Commands.Revoke;

public class RevokeTokenValidator : AbstractValidator<RevokeTokenCommand>
{
    public RevokeTokenValidator(IValidationRulesProvider rules)
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .Must((c, _) => rules.IsValidEmail(c.Email))
            .WithMessage("You passed an invalid email.");
    }
}