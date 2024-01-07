using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Operation.StatusCode;

[ExcludeFromCodeCoverage]
public class AuthOperationFilter : ProblemDetailsOperationFilter
{
    public override void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        context.ApiDescription.TryGetMethodInfo(out var methodInfo);
        if ( methodInfo is not { DeclaringType: not null, MemberType: MemberTypes.Method } ) return;

        // Check the controller or the method itself has no Auth required
        // (AllowAnonymous because can be different type of authorization besides Authorize inheritance)
        var isPublicAction = methodInfo
            .DeclaringType.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>()
            .Union(methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>()).Any();
        if ( isPublicAction ) return;

        // This adds the "Padlock" icon to the endpoint in swagger,
        // we can also pass through the names of the policies in the List<string>()
        // which will indicate which permission you require.
        operation.AddSecurityPadlock();

        var instance = BuildInstance(operation, context);

        operation.AddProblemDetailsResponse(context,
            StatusCodes.Status401Unauthorized,
            "If access token is not provided or invalid.",
            instance: instance);

        operation.AddProblemDetailsResponse(context,
            StatusCodes.Status403Forbidden,
            "If provided credentials do not grant access to this resource.",
            instance: instance);
    }
}
