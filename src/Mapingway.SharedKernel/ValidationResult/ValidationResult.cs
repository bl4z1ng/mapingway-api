using Mapingway.SharedKernel.Result;

namespace Mapingway.SharedKernel.ValidationResult;

public class ValidationResult : Result.Result, IValidationResult
{
    private ValidationResult(Error[] errors) : base(false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors)
    {
        return new ValidationResult(errors);
    }
}