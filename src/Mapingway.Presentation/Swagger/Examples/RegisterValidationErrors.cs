using System.Diagnostics.CodeAnalysis;
using Mapingway.Presentation.Swagger.Filters.Utility;
using Mapingway.SharedKernel.Result;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger.Examples;

[ExcludeFromCodeCoverage]
public class RegisterValidationErrors : IMultipleExamplesProvider<ExampleProblemDetails>
{
    public IEnumerable<SwaggerExample<ExampleProblemDetails>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "One or more validation errors occur.",
            new ExampleProblemDetails(StatusCodes.Status422UnprocessableEntity)
            {
                Detail = $"{DefaultErrorCode.InvalidCredentials}",
                Instance = "api/v1/user/register/",
                Errors = new Dictionary<string, string[]>
                {
                    {"Email", ["User with such email is already registered."]}
                }
            });

        yield return SwaggerExample.Create(
            $"",
            new ExampleProblemDetails(StatusCodes.Status422UnprocessableEntity)
            {
                Detail = "One or more validation errors occur.",
                Instance = "api/v1/user/register/",
                Errors = new Dictionary<string, string[]>
                {
                    {"Email", ["Email must be valid.", "Password should contain at least 3 letters."]}
                }
            });
    }
}
