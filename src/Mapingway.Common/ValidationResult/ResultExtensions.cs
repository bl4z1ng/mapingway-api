namespace Mapingway.Common.ValidationResult;

public static class ResultExtensions
{
    public static bool IsValidationResult(this Result.Result result)
    {
        return result is IValidationResult;
    }
}