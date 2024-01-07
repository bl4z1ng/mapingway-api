using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Utility;

[ExcludeFromCodeCoverage]
public static class OpenApiSchemaExtensions
{
    public static OpenApiResponse BuildOpenApiResponse<T>(
        this OperationFilterContext context,
        string description,
        T response) where T : class
    {
        var schema = GetOpenApiSchema<T>(context);

        schema.Example = response switch
        {
            ProblemDetails details => details.ToOpenApiObject(),
            _ => throw new InvalidOperationException($"Unable to map provided object to response, type: {typeof(T)}")
        };

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


    public static OpenApiSchema GetOpenApiSchema<T>(OperationFilterContext context)
    {
        // Try to get model from swagger schema repository
        var schemaNotFound = !context.SchemaRepository.TryLookupByType(typeof(T), out var schema);

        if ( schemaNotFound )
        {
            // Add schema inside it to exclude multiple adding
            schema = context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository);
        }

        return schema;
    }
}
