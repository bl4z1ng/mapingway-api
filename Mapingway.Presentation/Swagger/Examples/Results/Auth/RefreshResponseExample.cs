using Mapingway.Presentation.Controllers.Response;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger.Examples.Results.Auth;

public class RefreshResponseExample : IExamplesProvider<RefreshResponse>
{
    public RefreshResponse GetExamples()
    {
        return new RefreshResponse
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9." +
                    "eyJzdWIiOiIyIiwiZW1haWwiOiJtYXgucHl0Z." +
                    "f3NJ8swoeJIJajwdYv_cfC6lPXSLSYuj2d4PaXaLp3A",
            RefreshToken = "PwYuoPqGtW+Jd5aZJWrzUw=="
        };
    }
}