using Mapingway.Presentation.v1.Auth.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger.Examples.Requests.User;

public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
    {
        return new LoginRequest
            {
                Email = "max.pyte@gmail.com",
                Password = "Password"
            };
    }
}