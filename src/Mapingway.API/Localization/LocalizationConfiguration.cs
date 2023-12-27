using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.API.Localization;

[ExcludeFromCodeCoverage]
public static class LocalizationConfiguration
{
    public static string DefaultCulture => "en-US";
    public static List<string> SupportedCultures => new() { "en-GB", "en", "fr-FR" };

    public static IServiceCollection AddLocalizationRules(
        this IServiceCollection services,
        string? defaultCulture = null,
        List<string>? supportedCultures = null)
    {
        services.AddLocalization();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.RequestCultureProviders.Clear();
            options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());

            options.ApplyCurrentCultureToResponseHeaders = true;
            options.FallBackToParentCultures = true;
            options.CultureInfoUseUserOverride = false;

            defaultCulture ??= DefaultCulture;
            options.SetDefaultCulture(defaultCulture);

            supportedCultures ??= SupportedCultures;
            if (!supportedCultures.Contains(defaultCulture))
            {
                supportedCultures.Add(defaultCulture);
            }

            options.AddSupportedUICultures(supportedCultures.ToArray());
            options.AddSupportedCultures(supportedCultures.ToArray());
        });

        return services;
    }
}
