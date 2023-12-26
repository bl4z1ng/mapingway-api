using System.Reflection;
using Mapingway.Application.Behaviors;
using Mapingway.Application.Contracts.Validation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Application.AssemblyReference);
        });

        return builder.Services
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