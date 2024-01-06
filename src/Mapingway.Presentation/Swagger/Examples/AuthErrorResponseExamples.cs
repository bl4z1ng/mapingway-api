using Mapingway.SharedKernel.Result;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger.Examples;

public class RefreshToken400ErrorResponseExample : IExamplesProvider<Error>
{
    public Error GetExamples()
    {
        return new Error(
            DefaultErrorCode.InvalidCredentials,
            "Refresh token is invalid, try to login again");
    }
}

public class LogoutToken400ErrorResponseExample : IExamplesProvider<Error>
{
    public Error GetExamples()
    {
        return new Error(
            DefaultErrorCode.InvalidCredentials,
            "Access token is invalid");
    }
}

//TODO: use or remove
public class RevokeToken404ErrorResponseExample : IExamplesProvider<Error>
{
    public Error GetExamples()
    {
        return new Error(
            DefaultErrorCode.NotFound,
            "User has no active refresh token, try to log-in again");
    }
}
