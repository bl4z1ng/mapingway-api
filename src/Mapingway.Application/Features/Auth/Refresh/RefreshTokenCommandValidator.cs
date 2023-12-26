using FluentValidation;

namespace Mapingway.Application.Features.Auth.Refresh;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(c => c.ExpiredToken)
            .NotEmpty();

        RuleFor(c => c.RefreshToken)
            .NotEmpty();
    }
}