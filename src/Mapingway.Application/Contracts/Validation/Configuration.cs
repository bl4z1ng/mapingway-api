using FluentValidation;
using Mapingway.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Application.Contracts.Validation;

public static class Configuration
{
    internal static IServiceCollection AddValidationBehavior(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddScoped(
            typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        services.AddValidatorsFromAssemblyContaining(typeof(Application), includeInternalTypes: true);

        return services;
    }
}