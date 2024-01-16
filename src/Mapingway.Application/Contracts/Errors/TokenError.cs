using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Contracts.Errors;

public static class TokenError
{
    public static Error NotFound => new(DefaultErrorCode.NotFound, "Provided refresh token was not found.");
    public static Error Expired => new("Token.Expired", "Provided refresh token is expired.");
    public static Error FailedToGenerate => new("Token.FailedToGenerate", "Could not generate token with given data.");
}
