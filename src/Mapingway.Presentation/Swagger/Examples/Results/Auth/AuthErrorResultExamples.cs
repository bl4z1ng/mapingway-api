using Mapingway.Common.Result;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger.Examples.Results.Auth;

public class Authentication401ErrorResultExample : IMultipleExamplesProvider<Error>
{
    public IEnumerable<SwaggerExample<Error>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Invalid Credentials",
            new Error(
                ErrorCode.InvalidCredentials, 
                "Email or password are incorrect."));

        yield return SwaggerExample.Create(
            "No Active Refresh Token",
            new Error(
                ErrorCode.RefreshTokenIsInvalid, 
                "Refresh token is invalid, try to login again"));
    }
}

public class RefreshToken400ErrorResultExample : IExamplesProvider<Error>
{
    public Error GetExamples()
    {
        return new Error(
            ErrorCode.RefreshTokenIsInvalid,
            "Refresh token is invalid, try to login again");
    }
}

public class LogoutToken400ErrorResultExample : IExamplesProvider<Error>
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