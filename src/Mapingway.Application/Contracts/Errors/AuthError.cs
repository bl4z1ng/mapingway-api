using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Contracts.Errors;

public static class AuthError
{
    public static Error InvalidPassword => new(ErrorCode.InvalidCredentials, "Password is incorrect.");
}
