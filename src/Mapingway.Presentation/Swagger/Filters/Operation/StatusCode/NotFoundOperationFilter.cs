using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Mapingway.Presentation.v1;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

using Microsoft.AspNetCore.Http;

namespace Mapingway.Presentation.Swagger.Filters.Operation.StatusCode;

[ExcludeFromCodeCoverage]
public class NotFoundOperationFilter : ProblemDetailsOperationFilter
{
    public override void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        context.ApiDescription.TryGetMethodInfo(out var methodInfo);
        if ( methodInfo is not { DeclaringType: not null, MemberType: MemberTypes.Method } ) return;

        var routeTemplate = context.ApiDescription.RelativePath;

        var isIdAction = routeTemplate?.EndsWith(Routes.Id) ?? false;
        if ( !isIdAction ) return;

        var instance = BuildInstance(operation, routeTemplate!);

        operation.AddProblemDetailsResponse(
            context,
            StatusCodes.Status404NotFound,
            "If requested resource was not found or changed it's location.",
            instance: instance);
    }
}
