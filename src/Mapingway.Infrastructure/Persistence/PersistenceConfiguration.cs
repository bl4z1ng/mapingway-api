﻿using Mapingway.Application.Contracts.Authentication;
using Mapingway.Domain.Auth;
using Mapingway.Infrastructure.Persistence.Context;
using Mapingway.Infrastructure.Persistence.Repositories;
using Mapingway.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mapingway.Infrastructure.Persistence;

public static class PersistenceConfiguration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IHostEnvironment environment)
    {
        services.ConfigureOptions<DbOptionsSetup>().AddRepositoriesAndUnitOfWork();

        if (environment.IsDevelopment())
        {
            services.AddDbContext<DbContext, DevelopmentDbContext>();
        }
        else
        {
            services.AddDbContext<DbContext, ApplicationDbContext>();
        }

        return services;
    }

    private static IServiceCollection AddRepositoriesAndUnitOfWork(this IServiceCollection services)
    {
        return services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IPermissionRepository, PermissionRepository>()
            .AddScoped<IRepository<RefreshToken>, RefreshTokenRepository>()
            .AddScoped<IRepository<RefreshTokenFamily>, UsedRefreshTokenFamilyRepository>()

            .AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
