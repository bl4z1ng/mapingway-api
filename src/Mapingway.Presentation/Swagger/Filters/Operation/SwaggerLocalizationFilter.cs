using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mapingway.Presentation.Swagger.Filters.Operation
{
    [ExcludeFromCodeCoverage]
    public class SwaggerLocalizationFilter : IOperationFilter
    {
        private readonly IStringLocalizer<SwaggerLocalizationFilter> _localizer;
        private readonly RequestLocalizationOptions _options;

        public SwaggerLocalizationFilter(
            IStringLocalizer<SwaggerLocalizationFilter> localizer,
            IOptions<RequestLocalizationOptions> localizationOptions)
        {
            _localizer = localizer;
            _options = localizationOptions.Value;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            AddSupportedLanguagesDropdown(operation);
        }

        private void AddSupportedLanguagesDropdown(OpenApiOperation operation)
        {
            var supportedLanguages =
                _options.SupportedCultures ?? new List<CultureInfo> { _options.DefaultRequestCulture.Culture };

            IEnumerable<IOpenApiAny> enumList = supportedLanguages.Select(culture => new OpenApiString(culture.Name));
            var languageParameter = new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Description = _localizer["Select Language"],
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = enumList.ToList(),
                    Default = new OpenApiString(_options.DefaultRequestCulture.Culture.Name),
                },
            };

            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Insert(0, languageParameter);
        }
    }
}
