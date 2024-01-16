using FluentValidation;

namespace Mapingway.Application.Contracts.Validation;

public static class ValidationRules
{
    public static IRuleBuilder<T, string> InclusiveBetween<T>(
        this IRuleBuilder<T, string> rule,
        int? minLength = null,
        int? maxLength = null)
    {
        if (minLength != null) rule.MinimumLength(minLength.Value);
        if (maxLength != null) rule.MaximumLength(maxLength.Value);

        return rule;
    }
}
