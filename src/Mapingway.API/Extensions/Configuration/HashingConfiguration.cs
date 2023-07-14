using Mapingway.API.OptionsSetup;
using Mapingway.Application.Abstractions;
using Mapingway.Infrastructure.Security;

namespace Mapingway.API.Extensions.Configuration;

public static class HashingConfiguration
{
    public static IServiceCollection ConfigureHashing(this IServiceCollection services)
    {
        services.ConfigureOptions<HashOptionsSetup>();
        services.AddScoped<IHasher, Hasher>();

        return services;
    }
}