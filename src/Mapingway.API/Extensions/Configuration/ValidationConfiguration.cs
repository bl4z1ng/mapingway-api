using FluentValidation;
using Mapingway.API.OptionsSetup.Validation;
using Mapingway.Application;
using Mapingway.Application.Abstractions.Validation;
using Mapingway.Application.Behaviors;
using Mapingway.Infrastructure.Validation.Email;
using Mapingway.Infrastructure.Validation.Password;
using MediatR;

namespace Mapingway.API.Extensions.Configuration;

public static class ValidationConfiguration
{
    public static IServiceCollection ConfigureValidation(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddScoped(
            typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services.ConfigureOptions<PasswordValidationRulesSetup>();
        services.AddScoped<IPasswordValidationRulesProvider, PasswordValidationRulesProvider>();

        services.ConfigureOptions<EmailValidationRulesSetup>();
        services.AddScoped<IEmailValidationRulesProvider, EmailValidationRulesProvider>();

        services.AddValidatorsFromAssembly(ApplicationAssembly.AssemblyReference);

        return services;
    }
}