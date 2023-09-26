﻿using FluentValidation;
using Mapingway.Application.Contracts.Abstractions.Validation;

namespace Mapingway.Application.Auth.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator(
        IEmailValidationRulesProvider emailRules,
        IPasswordValidationRulesProvider passwordRules)
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .Must((c, _) => emailRules.IsEmailValid(c.Email))
            .WithMessage("Email is invalid.");

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(30)
            .Must((c, _) => passwordRules.HasNOrMoreLetters(c.Password))
                .WithMessage($"Password must have at least {passwordRules.NumberOfLetters} letters.");
    }
}