using System.Net.Mime;
using Mapingway.Presentation.Swagger.Filters.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Operation;

public static class OpenApiOperationStatusCodesExtensions
{
    public static void AddStatusCodeResponse(
        this OpenApiOperation method,
        OperationFilterContext context,
        int statusCode,
        string swaggerDescription,
        string statusCodesUrl = "https://httpstatuses.io/")
    {
        var problemDetails = new ProblemDetails
        {
            Type = statusCodesUrl + statusCode,
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Status = statusCode,
            Extensions = new Dictionary<string, object?>
            {
                { "TraceId", "00-de0323c58d949722feda08ca733e1e0f-08519d60c4a1874c-00" }
            }
        };

        var statusCodeResponse = BuildOpenApiResponse(
            context,
            description: swaggerDescription,
            response: problemDetails);

        method.Responses.Add(statusCode.ToString(), statusCodeResponse);
    }

    private static OpenApiResponse BuildOpenApiResponse<T>(
        OperationFilterContext context,
        string description,
        T response) where T : class
    {
        var schema = GetOpenApiSchema<T>(context);

        schema.AddResponseExample(response);

        var content = new Dictionary<string, OpenApiMediaType>
        {
            { MediaTypeNames.Application.Json, new OpenApiMediaType { Schema = schema, Example = schema.Example } }
        };

        return new OpenApiResponse
        {
            Description = description,
            Content = content
        };
    }

    private static OpenApiSchema GetOpenApiSchema<T>(OperationFilterContext context)
    {
        // Try to get model from swagger schema repository
        var schemaNotFound = !context.SchemaRepository.TryLookupByType(typeof(T), out var schema);

        if (schemaNotFound)
        {
            // Add schema inside of it to exclude multiple addition
            schema = context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository);
        }

        return schema;
    }
}
