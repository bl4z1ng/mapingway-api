using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Contracts.Errors;

public static class UserError
{
    public static Error NotFound => new(ErrorCode.NotFound, "User with provided data was not found.");
}
