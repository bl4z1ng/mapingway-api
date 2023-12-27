using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Operation;

[ExcludeFromCodeCoverage]
public static class SwaggerGenOperationFilterExtensions
{
    public static SwaggerGenOptions AddCommonStatusCodesResponses(this SwaggerGenOptions options)
    {
        options.OperationFilter<InternalOperationFilter>();
        options.OperationFilter<AuthOperationFilter>();

        return options;
    }
}
