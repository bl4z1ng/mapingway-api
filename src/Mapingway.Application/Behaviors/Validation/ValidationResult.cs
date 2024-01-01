using FluentValidation.Results;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Behaviors.Validation;

public class ValidationResult : Result, IValidationResult
{
    public static ValidationResult WithFailures(IEnumerable<ValidationFailure> failures)
    {
        var groupedValidationMessages = failures
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).ToArray());

        return new ValidationResult(groupedValidationMessages);
    }

    public ValidationResult(IDictionary<string, string[]> failures) : base(false, IValidationResult.ValidationError)
    {
        Failures = failures;
    }

    public IDictionary<string, string[]> Failures { get; }
}
