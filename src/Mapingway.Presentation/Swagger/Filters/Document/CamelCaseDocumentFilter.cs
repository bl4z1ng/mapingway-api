using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Document;

[ExcludeFromCodeCoverage]
public class CamelCaseDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var dictionaryPaths = swaggerDoc.Paths.ToDictionary(
            x => ToCamelCase(x.Key),
            x => x.Value);

        var newPaths = new OpenApiPaths();
        foreach (var (key, value) in dictionaryPaths)
        {
            newPaths.Add(key, value);
        }

        swaggerDoc.Paths = newPaths;
    }

    private static string ToCamelCase(string key)
    {
        var parts = key.Split('/').Select(part => part.Contains('}') ?
            part :
            JsonNamingPolicy.CamelCase.ConvertName(part));

        return string.Join('/', parts);
    }
}
