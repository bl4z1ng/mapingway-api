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

    [System.Text.RegularExpressions.GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    private static partial System.Text.RegularExpressions.Regex Email();
}