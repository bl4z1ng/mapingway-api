using Mapingway.Application.Contracts.User.Result;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Swagger.Examples.Responses.User;

public class RegisterResultExample : IExamplesProvider<RegisterResult>
{
    public RegisterResult GetExamples()
    {
        return new RegisterResult
        {
            Email = "max.pyte@gmail.com",
            FirstName = "Max",
            LastName = "Pyte"
        };
    }
}