using System.Reflection;
using Mapingway.Infrastructure.Authentication;
using Mapingway.Infrastructure.Logging.ProblemDetails;
using Mapingway.Infrastructure.Persistence;
using Mapingway.Infrastructure.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddHashing();
        services.AddAuth();
        services.AddPersistence(environment);

        services.AddScoped<IProblemDetailsFactory, CustomProblemDetailsFactory>();

        return services;
    }
}

public class Infrastructure
{
    public static readonly Assembly AssemblyReference = typeof(Infrastructure).Assembly;
}

public static class ServiceCollectionOptionsExtensions
{
    public static IServiceCollection AddValidatedOptions<TOptions>(
        this IServiceCollection services,
        string configurationSection) where TOptions : class
    {
        services
            .AddOptionsWithValidateOnStart<TOptions>()
            .ValidateDataAnnotations()
            .BindConfiguration(configurationSection);

        return services;
    }
}
