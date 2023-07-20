using FluentValidation;
using Mapingway.API.OptionsSetup;
using Mapingway.Application;
using Mapingway.Application.Abstractions.Validation;
using Mapingway.Application.Behaviors;
using Mapingway.Infrastructure.Validation.Email;
using Mapingway.Infrastructure.Validation.Password;
using MediatR;

namespace Mapingway.API.Extensions.Configuration;

public static class ValidationConfiguration
{
    public static WebApplicationBuilder ConfigureValidation(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddScoped(
            typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services
            .AddOptions<PasswordValidationRules>()
            .Bind(configuration.GetSection(
                $"{ValidationOptions.ConfigurationSection}:{PasswordValidationRules.ConfigurationSection}"))
            .ValidateOnStart();
        services.AddScoped<IPasswordValidationRulesProvider, PasswordValidationRulesProvider>();

        services
            .AddOptions<EmailValidationRules>()
            .Bind(configuration.GetSection(
                $"{ValidationOptions.ConfigurationSection}:{EmailValidationRules.ConfigurationSection}"))
            .ValidateOnStart();
        services.AddScoped<IEmailValidationRulesProvider, EmailValidationRulesProvider>();

        services.AddValidatorsFromAssembly(ApplicationAssembly.AssemblyReference);

        return builder;
    }
}