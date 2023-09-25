﻿using FluentValidation;
using Mapingway.Application.Abstractions.Validation;

namespace Mapingway.Application.Auth.Commands.Logout;

public class LogoutTokenValidator : AbstractValidator<LogoutTokenCommand>
{
    public LogoutTokenValidator(IEmailValidationRulesProvider emailRules)
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .Must((c, _) => emailRules.IsEmailValid(c.Email))
            .WithMessage("You passed an invalid email.");
    }
}