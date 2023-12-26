using Mapingway.Application.Contracts.Abstractions;
using Mapingway.Application.Contracts.Authentication;
using Mapingway.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mapingway.Infrastructure.Persistence;

public static class PersistenceConfiguration
{
    public static IServiceCollection AddPersistance(this IServiceCollection services)
    {
        services
            .ConfigureOptions<DbOptionsSetup>()
            .AddRepositoriesAndUnitOfWork();

#if DEBUG
        services.AddDbContext<DbContext, DevelopmentDbContext>();
#else 
            services.AddDbContext<DbContext, ApplicationDbContext>();
#endif

        return services;
    }

    private static IServiceCollection AddRepositoriesAndUnitOfWork(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IPermissionRepository, PermissionRepository>()
            .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>()
            .AddScoped<IUsedRefreshTokenFamilyRepository, UsedRefreshTokenFamilyRepository>()

            .AddScoped<IUnitOfWork, UnitOfWork>();
    }
}