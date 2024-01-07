using FluentValidation;
using Mapingway.Application.Contracts.Validation;

namespace Mapingway.Application.Features.Auth.Refresh;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().ValidEmail();

        RuleFor(c => c.RefreshToken).NotEmpty();
    }
}
