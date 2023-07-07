using Mapingway.Application.Contracts.User.Result;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Swagger.Examples.Responses.User;

public class AuthenticationResultExample : IExamplesProvider<AuthenticationResult>
{
    public AuthenticationResult GetExamples()
    {   
        return new AuthenticationResult
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                    "eyJzdWIiOiIyIiwiZW1haWwiOiJtYXgucHl0Z." +
                    "f3NJ8swoeJIJajwdYv_cfC6lPXSLSYuj2d4PaXaLp3A",
            RefreshToken = "PwYuoPqGtW+Jd5aZJWrzUw=="
        };
    }
}