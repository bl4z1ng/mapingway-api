using System.Reflection;
using Mapingway.Infrastructure.Authentication;
using Mapingway.Infrastructure.Persistence;
using Mapingway.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mapingway.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddPersistence(environment);
        services.AddHashing(configuration);
        services.AddAuth(configuration);

        return services;
    }
}

public class Infrastructure
{
    public static readonly Assembly AssemblyReference = typeof(Infrastructure).Assembly;
}
