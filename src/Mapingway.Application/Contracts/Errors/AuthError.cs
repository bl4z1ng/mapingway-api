using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Contracts.Errors;

public static class AuthError
{
    public static Error InvalidPassword => new(DefaultErrorCode.InvalidCredentials, "Password is incorrect.");
}
