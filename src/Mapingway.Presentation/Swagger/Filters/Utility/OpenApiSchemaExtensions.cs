using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;

namespace Mapingway.Presentation.Swagger.Filters.Utility;

[ExcludeFromCodeCoverage]
public static class OpenApiSchemaExtensions
{
    public static void AddResponseExample<T>(this OpenApiSchema schema, T content) where T : class
    {
        schema.Example = ClassToOpenApiObjectMapper.Map(content, namesToLowerInvariant: true);
    }
}
