using Mapingway.Common.Result.Validation;

namespace Mapingway.Common.Result;

public static class ResultExtensions
{
    public static bool IsValidationResult(this Result result)
    {
        return result is IValidationResult;
    }
}