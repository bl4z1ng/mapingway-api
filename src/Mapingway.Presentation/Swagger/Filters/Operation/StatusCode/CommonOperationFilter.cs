using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Operation.StatusCode;

[ExcludeFromCodeCoverage]
public class CommonOperationFilter : ProblemDetailsOperationFilter
{
    public override void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var instance = BuildInstance(operation, context);

        operation.AddProblemDetailsResponse(context,
            StatusCodes.Status500InternalServerError,
            "Something is going wrong on our side, please try again or try later.",
            instance: instance);

        operation.AddProblemDetailsResponse(context,
            StatusCodes.Status503ServiceUnavailable,
            "Something is going wrong on our side, please try again or try later.",
            instance: instance);
    }
}
