using FluentValidation.Results;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Behaviors.Validation;

public record ValidationError : Error
{
    public const string DefaultMessage = "One or more validation errors occur.";

    public ValidationError(IDictionary<string, string[]> failures, string? message = null)
        : base(ErrorCode.ValidationError, message ?? DefaultMessage)
    {
        Failures = failures;
    }

    public IDictionary<string, string[]> Failures { get; }

    public static ValidationError WithFailures(IEnumerable<ValidationFailure> failures, string? message = null)
    {
        var groupedValidationMessages = failures
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).ToArray());

        return new ValidationError(groupedValidationMessages, message);
    }
}
