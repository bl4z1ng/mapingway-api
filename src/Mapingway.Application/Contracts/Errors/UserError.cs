using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Contracts.Errors;

public static class UserError
{
    public static Error NotFound => new(DefaultErrorCode.NotFound, "User with provided data was not found.");
}
