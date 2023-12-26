using System.Reflection;
using Mapingway.Application.Behaviors;
using Mapingway.Application.Contracts.Validation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Application.AssemblyReference);
        });

        return services
            .ConfigureLoggingBehavior()
            .ConfigureValidationBehavior();
    }


    //TODO: remove
    internal static IServiceCollection ConfigureLoggingBehavior(this IServiceCollection services)
    {
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingPipelineBehavior<,>));

        return services;
    }
}

public class Application
{
    public static readonly Assembly AssemblyReference = typeof(Application).Assembly;
}