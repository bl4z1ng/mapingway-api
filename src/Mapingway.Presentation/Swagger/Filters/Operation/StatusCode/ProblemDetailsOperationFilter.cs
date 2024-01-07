using System.Diagnostics.CodeAnalysis;
using Mapingway.Presentation.Swagger.Filters.Utility;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Operation.StatusCode;

[ExcludeFromCodeCoverage]
public abstract class ProblemDetailsOperationFilter : IOperationFilter
{
    public abstract void Apply(OpenApiOperation operation, OperationFilterContext context);


    protected static string? BuildInstance((string Name, string Value)[] parameters, string routeTemplate)
    {
        return FillInRouteParameters(routeTemplate.ToCamelCase(), parameters);
    }

    protected static string? BuildInstance(OpenApiOperation operation, string routeTemplate)
    {
        var parameterPlaceholders = GetRouteParameters(operation);

        return BuildInstance(parameterPlaceholders, routeTemplate);
    }

    protected static string? BuildInstance(OpenApiOperation operation, OperationFilterContext context)
    {
        var routeTemplate = context.ApiDescription.RelativePath;
        if ( routeTemplate is null ) return routeTemplate;

        return BuildInstance(operation, routeTemplate);
    }

    #region Utility

    private static (string Name, string Example)[] GetRouteParameters(OpenApiOperation operation)
    {
        return operation.Parameters.Select(p =>
            (
                Name: $"{{{p.Name}}}",
                Value: ( p.Example as OpenApiString )?.Value ?? string.Empty)
        ).ToArray();
    }

    private static string FillInRouteParameters(string route, params (string Placeholder, string Value)[] templates)
    {
        foreach ( var template in templates )
            route = route.Replace(template.Placeholder, template.Value);

        return route;
    }

    #endregion
}
