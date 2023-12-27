using System.Text.RegularExpressions;
using FluentValidation;

namespace Mapingway.Application.Contracts.Validation;

public static partial class EmailValidationRules
{
    public static IRuleBuilderOptions<T, string> ValidEmail<T>(this IRuleBuilder<T, string> rule)
    {
        return rule
            .Must((_, email) => Email().IsMatch(email))
            .WithMessage("Email is invalid.");
    }

    [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    private static partial Regex Email();
}