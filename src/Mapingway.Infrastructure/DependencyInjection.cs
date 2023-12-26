using System.Reflection;
using Mapingway.Infrastructure.Authentication;
using Mapingway.Infrastructure.Persistence;
using Mapingway.Infrastructure.Security;
using Mapingway.Infrastructure.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistance();

        services.AddValidationRules(configuration);

        services.AddHashing(configuration);

        services.AddAuth(configuration);

        return services;
    }
}

public class Infrastructure
{
    public static readonly Assembly AssemblyReference = typeof(Infrastructure).Assembly;
}