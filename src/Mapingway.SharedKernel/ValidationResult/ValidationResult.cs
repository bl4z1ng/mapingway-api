using Mapingway.SharedKernel.Result;

namespace Mapingway.SharedKernel.ValidationResult;

public class ValidationResult : Result.Result, IValidationResult
{
    public Error[] Errors { get; }

    private ValidationResult(Error[] errors) : base(false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public static ValidationResult WithErrors(Error[] errors)
    {
        return new ValidationResult(errors);
    }
}