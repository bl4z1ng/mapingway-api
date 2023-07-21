using System.Reflection;
using FluentValidation;
using Mapingway.API.OptionsSetup;
using Mapingway.Application;
using Mapingway.Application.Abstractions.Validation;
using Mapingway.Application.Behaviors;
using Mapingway.Infrastructure.Validation;
using Mapingway.Infrastructure.Validation.Email;
using Mapingway.Infrastructure.Validation.Password;
using MediatR;

namespace Mapingway.API.Extensions.Configuration;

public static class ValidationConfiguration
{
    public static WebApplicationBuilder ConfigureValidation(this WebApplicationBuilder builder)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        builder.Services.AddScoped(
            typeof(IPipelineBehavior<,>), 
            typeof(ValidationPipelineBehavior<,>));

        builder
            .AddValidationRules<PasswordValidationRules>()
            .AddScoped<IPasswordValidationRulesProvider, PasswordValidationRulesProvider>();

        builder
            .AddValidationRules<PasswordValidationRules>()
            .AddScoped<IEmailValidationRulesProvider, EmailValidationRulesProvider>();

        builder.Services.AddValidatorsFromAssembly(ApplicationAssembly.AssemblyReference);

        return builder;
    }

    private static IServiceCollection AddValidationRules<TRules>(this WebApplicationBuilder builder) 
        where TRules : class, IValidationRules
    {
        var configurationSectionProperty = typeof(TRules).GetProperty(
            nameof(IValidationRules.ConfigurationSection), BindingFlags.Public | BindingFlags.Static);

        builder.Services
            .AddOptions<TRules>()
            .Bind(builder.Configuration.GetSection(
                $"{ValidationOptions.ConfigurationSection}:{configurationSectionProperty!.GetValue(null)}"))
            .ValidateOnStart();

        return builder.Services;
    }
}