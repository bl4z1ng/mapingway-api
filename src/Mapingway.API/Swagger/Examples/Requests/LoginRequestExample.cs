using Mapingway.Application.Contracts.User;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Swagger.Examples.Requests;

public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
    {
        return new LoginRequest
            {
                Email = "valid.email.@gmail.com",
                Password = "eh*34Gr68J"
            };
    }
}