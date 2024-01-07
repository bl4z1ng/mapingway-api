using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Operation;

public class CorrelationTokenFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        AddCorrelationTokenHeader(operation);
    }
    private void AddCorrelationTokenHeader(OpenApiOperation operation)
    {
        var tokenParameter = new OpenApiParameter
        {
            Name = "Correlation-Id",
            Description = "Unique-per-request correlation token that can be used for troubleshooting and distributed tracing.",
            In = ParameterLocation.Header,
            Required = false,
            Schema = new OpenApiSchema
            {
                Type = "string"
            },
        };
        operation.Parameters.Insert(0, tokenParameter);
    }
}
