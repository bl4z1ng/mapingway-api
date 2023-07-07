using Mapingway.Common.Result;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Swagger.Examples.Responses.Token;

public class RefreshToken400ErrorResultExample : IExamplesProvider<Error>
{
    public Error GetExamples()
    {
        return new Error(
            ErrorCode.RefreshTokenIsInvalid,
            "Refresh token is invalid, try to login again");
    }
}

public class RevokeToken400ErrorResultExample : IExamplesProvider<Error>
{
    public Error GetExamples()
    {
        return new Error(
            ErrorCode.InvalidCredentials,
            "Access token is invalid");
    }
}

public class RevokeToken404ErrorResultExample : IExamplesProvider<Error>
{
    public Error GetExamples()
    {
        return new Error(
            ErrorCode.NotFound,
            "User has no active refresh token, try to log-in again");
    }
}