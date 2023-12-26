using System.Reflection;
using Mapingway.Application.Contracts.Validation;
using Mapingway.Infrastructure.Validation.Email;
using Mapingway.Infrastructure.Validation.Name;
using Mapingway.Infrastructure.Validation.Password;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Infrastructure.Validation;

public static class Configuration
{
    public static IServiceCollection AddValidationRules(this IServiceCollection services, IConfiguration configuration)
    {
        //TODO: improve or remove
        services
            .AddValidationRules<NameValidationRules>(configuration)
            .AddScoped<INameValidationRulesProvider, NameValidationRulesProvider>();

        services
            .AddValidationRules<PasswordValidationRules>(configuration)
            .AddScoped<IPasswordValidationRulesProvider, PasswordValidationRulesProvider>();

        services
            .AddValidationRules<EmailValidationRules>(configuration)
            .AddScoped<IEmailValidationRulesProvider, EmailValidationRulesProvider>();

        return services;
    }

    private static IServiceCollection AddValidationRules<TRules>(this IServiceCollection services, IConfiguration configuration) 
        where TRules : class, IValidationRules
    {
        var configurationSectionProperty = typeof(TRules).GetProperty(
            nameof(IValidationRules.ConfigurationSection), BindingFlags.Public | BindingFlags.Static);

        services
            .AddOptions<TRules>()
            .Bind(configuration.GetSection(
                $"{ValidationOptions.ConfigurationSection}:{configurationSectionProperty!.GetValue(null)}"))
            .ValidateOnStart();

        return services;
    }
}