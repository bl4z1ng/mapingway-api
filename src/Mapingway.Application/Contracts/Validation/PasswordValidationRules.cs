using System.Text.RegularExpressions;
using FluentValidation;

namespace Mapingway.Application.Contracts.Validation;

public static partial class PasswordValidationRules
{
    public static IRuleBuilderOptions<T, string> HasLetters<T>(this IRuleBuilder<T, string> rule, int numberOfLetters = 3)
    {
        //TODO: apply number
        return rule
            .Must(password => LettersRegex().IsMatch(password))
            .WithMessage((_, _) => $"The password must contain at least {numberOfLetters} letters.");
    }

    [GeneratedRegex("^(?:[^a-zA-Z]*[a-zA-Z]){3}.*$")]
    private static partial Regex LettersRegex();
}
