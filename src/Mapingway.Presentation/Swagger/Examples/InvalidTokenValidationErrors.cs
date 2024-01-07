using System.Diagnostics.CodeAnalysis;
using Mapingway.Presentation.Swagger.Filters.Utility;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;

namespace Mapingway.Presentation.Swagger.Examples;

[ExcludeFromCodeCoverage]
public class InvalidTokenValidationErrors : IMultipleExamplesProvider<ExampleProblemDetails>
{
    public IEnumerable<SwaggerExample<ExampleProblemDetails>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Invalid access token",
            new ExampleProblemDetails(StatusCodes.Status422UnprocessableEntity)
            {
                Detail = "One or more validation errors occur.",
                Errors = new Dictionary<string, string[]>
                {
                    {"expiredToken", [$"Provided access token was not valid, please, log-in again."]}
                }
            });

        yield return SwaggerExample.Create(
            "Email was not found",
            new ExampleProblemDetails(StatusCodes.Status422UnprocessableEntity)
            {
                Detail = "One or more validation errors occur.",
                Errors = new Dictionary<string, string[]>
                {
                    { "ExpiredToken", [$"Provided access token is not valid anymore, please, log-in again."] }
                }
            });
    }
}
