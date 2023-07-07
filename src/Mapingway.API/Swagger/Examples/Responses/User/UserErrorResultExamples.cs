using Mapingway.Common.Result;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Swagger.Examples.Responses.User;

public class Authentication401ErrorResultExample : IMultipleExamplesProvider<Error>
{
    public IEnumerable<SwaggerExample<Error>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Invalid Credentials",
            new Error(
                ErrorCode.InvalidCredentials, 
                "Email or password ais incorrect."));

        yield return SwaggerExample.Create(
            "No Active Refresh Token",
            new Error(
                ErrorCode.RefreshTokenIsInvalid, 
                "Refresh token is invalid, try to login again"));
    }
}

public class Register400ErrorResultExample : IExamplesProvider<Error>
{
    public Error GetExamples()
    {
        return new Error(
            ErrorCode.InvalidCredentials, 
            "User with such email is already registered");
    }
}