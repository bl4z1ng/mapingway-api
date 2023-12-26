using Mapingway.Application.Contracts.User;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger.Examples.Results.User;

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