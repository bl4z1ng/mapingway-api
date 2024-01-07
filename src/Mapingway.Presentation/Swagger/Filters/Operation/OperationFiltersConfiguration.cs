using System.Diagnostics.CodeAnalysis;
using Mapingway.Presentation.Swagger.Filters.Operation.StatusCode;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Operation;

[ExcludeFromCodeCoverage]
public static class StatusCodesExamplesConfiguration
{
    public static SwaggerGenOptions AddCommonStatusCodeResponses(this SwaggerGenOptions options)
    {
        options.OperationFilter<CommonOperationFilter>();
        options.OperationFilter<AuthOperationFilter>();
        options.OperationFilter<NotFoundOperationFilter>();

        return options;
    }

    public static SwaggerGenOptions AddAcceptLanguageDropdown(this SwaggerGenOptions options)
    {
        options.OperationFilter<SwaggerLocalizationFilter>();

        return options;
    }

    public static SwaggerGenOptions AddCorrelationTokenParameter(this SwaggerGenOptions options)
    {
        options.OperationFilter<CorrelationTokenFilter>();
        return options;
    }
}
