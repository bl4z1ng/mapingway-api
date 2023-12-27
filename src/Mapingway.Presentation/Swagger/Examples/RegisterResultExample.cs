using Mapingway.Application.Features.User.Register;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger.Examples;

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
