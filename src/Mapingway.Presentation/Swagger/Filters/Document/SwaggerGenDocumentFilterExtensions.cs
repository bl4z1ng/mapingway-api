using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Document;

[ExcludeFromCodeCoverage]
public static class SwaggerGenDocumentFilterExtensions
{
    public static void ConvertRoutesToCamelCase(this SwaggerGenOptions options)
    {
        options.DocumentFilter<CamelCaseDocumentFilter>();
    }
}
