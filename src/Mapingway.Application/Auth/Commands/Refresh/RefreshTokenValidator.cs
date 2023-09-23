using FluentValidation;

namespace Mapingway.Application.Auth.Commands.Refresh;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(c => c.ExpiredToken)
            .NotEmpty();

        RuleFor(c => c.RefreshToken)
            .NotEmpty();
    }
}