using Mapingway.Application.Contracts.User;
using Mapingway.Application.Contracts.User.Request;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Swagger.Examples.Requests;

public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
{
    public RegisterRequest GetExamples()
    {
        return new RegisterRequest
        {
            Email = "valid.email.@gmail.com",
            Password = "eh*34Gr68J",
            FirstName = "Max",
            LastName = "Pyte"
        };
    }
}