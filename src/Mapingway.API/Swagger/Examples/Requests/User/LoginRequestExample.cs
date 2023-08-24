using Mapingway.Application.Contracts.User.Request;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Swagger.Examples.Requests.User;

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