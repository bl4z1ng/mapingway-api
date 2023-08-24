using Mapingway.Application.Contracts.User.Request;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Swagger.Examples.Requests.User;

public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
{
    public RegisterRequest GetExamples()
    {
        return new RegisterRequest
        {
            Email = "max.pyte@gmail.com",
            Password = "Password",
            FirstName = "Max",
            LastName = "Pyte"
        };
    }
}