using Mapingway.Application.Contracts.Abstractions;
using Mapingway.Application.Contracts.Abstractions.Authentication;
using Mapingway.Infrastructure.Persistence.Repositories;

namespace Mapingway.API.Installers;

public static class RepositoriesAndUnitOfWorkInstaller
{
    public static IServiceCollection AddRepositoriesAndUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IUsedRefreshTokenFamilyRepository, UsedRefreshTokenFamilyRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}