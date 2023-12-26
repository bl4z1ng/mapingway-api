using FluentValidation;
using Mapingway.Application.Contracts.Validation;

namespace Mapingway.Application.Features.Auth.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .ValidEmail();

        RuleFor(c => c.Password)
            .NotEmpty()
            .HasLetters(3)
            .InclusiveBetween(8, 30);
    }
}