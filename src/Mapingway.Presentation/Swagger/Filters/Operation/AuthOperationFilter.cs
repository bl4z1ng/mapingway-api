using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Operation;

[ExcludeFromCodeCoverage]
public class AuthOperationFilter : IOperationFilter
{
    private const string _statusCodeUrl = "https://httpstatuses.io/";

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        context.ApiDescription.TryGetMethodInfo(out var methodInfo);
        if ( methodInfo == null || methodInfo.MemberType != MemberTypes.Method ) return;

        var isPublicAction = false;
        if ( methodInfo is { DeclaringType: not null } )
        {
            // Check the controller or the method itself has no Auth required
            // (AllowAnonymous because can be different type of authorization besides Authorize inheritance)
            isPublicAction =
                methodInfo.DeclaringType.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>()
                    .Union(methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>())
                    .Any();
        }
        if ( isPublicAction ) return;

        // Adds the "Padlock" icon to the endpoint in swagger,
        // we can also pass through the names of the policies in the List<string>()
        // which will indicate which permission you require.
        operation.Security = new List<OpenApiSecurityRequirement>{
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            }
        };

        operation.AddStatusCodeResponse(context,
            StatusCodes.Status401Unauthorized,
            "If access token is not provided or invalid.",
            _statusCodeUrl);

        operation.AddStatusCodeResponse(context,
            StatusCodes.Status403Forbidden,
            "If provided credentials do not grant access to this resource.",
            _statusCodeUrl);
    }
}
