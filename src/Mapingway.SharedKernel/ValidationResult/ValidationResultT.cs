using Mapingway.SharedKernel.Result;

namespace Mapingway.SharedKernel.ValidationResult;

public class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] errors) 
        : base(false, IValidationResult.ValidationError, default)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationResult<TValue> WithErrors(Error[] errors)
    {
        return new ValidationResult<TValue>(errors);
    }
}