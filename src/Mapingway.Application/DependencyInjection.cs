using System.Reflection;
using Mapingway.Application.Contracts.Validation;
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

        return services.AddValidationBehavior();
    }
}

public class Application
{
    public static readonly Assembly AssemblyReference = typeof(Application).Assembly;
}