using Mapingway.Application.Contracts.Token.Result;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Swagger.Examples.Results.Token;

public class RefreshTokenResultExample : IExamplesProvider<RefreshTokenResult>
{
    public RefreshTokenResult GetExamples()
    {
        return new RefreshTokenResult()
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                    "eyJzdWIiOiIyIiwiZW1haWwiOiJtYXgucHl0Z." +
                    "f3NJ8swoeJIJajwdYv_cfC6lPXSLSYuj2d4PaXaLp3A",
            RefreshToken = "PwYuoPqGtW+Jd5aZJWrzUw=="
        };
    }
}