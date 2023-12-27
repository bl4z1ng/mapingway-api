using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Operation;

[ExcludeFromCodeCoverage]
public class InternalOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.AddStatusCodeResponse(context,
            StatusCodes.Status500InternalServerError,
            "Something is going wrong on our side, please try again or try later.");
    }
}
