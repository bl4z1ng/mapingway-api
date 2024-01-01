using FluentValidation.Results;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Behaviors.Validation;

public class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    public IDictionary<string, string[]> Failures { get; }

    private ValidationResult(IDictionary<string, string[]> failures)
        : base(false, IValidationResult.ValidationError, default)
    {
        Failures = failures;
    }

    // ReSharper disable once UnusedMember.Global
    // Used by reflection
    public static ValidationResult<TValue> WithFailures(IEnumerable<ValidationFailure> failures)
    {
        var groupedValidationMessages = failures
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).ToArray());

        return new ValidationResult<TValue>(groupedValidationMessages);
    }
}
