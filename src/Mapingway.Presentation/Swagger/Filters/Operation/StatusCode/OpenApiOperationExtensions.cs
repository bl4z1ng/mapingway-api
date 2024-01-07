using System.Diagnostics.CodeAnalysis;
using Mapingway.Presentation.Swagger.Filters.Utility;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Operation.StatusCode;

[ExcludeFromCodeCoverage]
public static class OpenApiOperationExtensions
{
    public static void AddProblemDetailsResponse(this OpenApiOperation method,
        OperationFilterContext context,
        int statusCode,
        string swaggerDescription,
        string? type = null,
        string? title = null,
        string? instance = null,
        string? detail = null,
        string? traceId = null)
    {
        var problemDetails = new ExampleProblemDetails(
            statusCode: statusCode,
            type: type,
            instance: instance,
            title: title,
            detail: detail,
            traceId: traceId);

        var statusCodeResponse = context.BuildOpenApiResponse(swaggerDescription, response: problemDetails);

        method.Responses.Add(statusCode.ToString(), statusCodeResponse);
    }


    public static void AddSecurityPadlock(this OpenApiOperation operation, params string[] permissions)
    {
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            { new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                permissions
            }
        });
    }
}
