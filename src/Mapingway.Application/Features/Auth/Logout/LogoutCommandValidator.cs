using FluentValidation;
using Mapingway.Application.Contracts.Validation;

namespace Mapingway.Application.Features.Auth.Logout;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().ValidEmail();
    }
}