using Mapingway.Common.Result;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.API.Swagger.Examples.Results.User;

public class Register400ErrorResultExample : IExamplesProvider<Error>
{
    public Error GetExamples()
    {
        return new Error(
            ErrorCode.InvalidCredentials, 
            "User with such email is already registered");
    }
}