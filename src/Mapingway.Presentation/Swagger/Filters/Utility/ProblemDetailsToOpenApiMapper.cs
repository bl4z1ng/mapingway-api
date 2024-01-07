using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

namespace Mapingway.Presentation.Swagger.Filters.Utility;

[ExcludeFromCodeCoverage]
public static class ProblemDetailsToOpenApiMapper
{
    public static OpenApiObject ToOpenApiObject(this ProblemDetails details)
    {
        var result = new OpenApiObject();

        result
            .Add(nameof(ProblemDetails.Type), details.Type)
            .Add(nameof(ProblemDetails.Title), details.Title)
            .Add(nameof(ProblemDetails.Status), details.Status)
            .Add(nameof(ProblemDetails.Detail), details.Detail)
            .Add(nameof(ProblemDetails.Instance), details.Instance);

        if ( details is ExampleProblemDetails extendedDetails )
            result.Add(nameof(ExampleProblemDetails.TraceId), extendedDetails.TraceId);

        foreach ( var (key, value) in details.Extensions )
        {
            if ( value is not null ) result.Add(key, value);
        }

        return result;
    }

    private static OpenApiObject Add<T>(this OpenApiObject obj, string key, T? value)
    {
        if ( value is null ) return obj;
        IOpenApiAny prop = value switch
        {
            int num => new OpenApiInteger(num),
            string str => new OpenApiString(str),
            _ => new OpenApiNull()
        };
        if ( prop is OpenApiNull ) return obj;

        var propKey = JsonNamingPolicy.CamelCase.ConvertName(key);
        obj[propKey] = prop;

        return obj;
    }
}
