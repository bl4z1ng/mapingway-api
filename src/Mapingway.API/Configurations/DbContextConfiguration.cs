using Mapingway.API.OptionsSetup;
using Mapingway.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.API.Configurations;

public static class DbContextConfiguration
{
    public static WebApplicationBuilder ConfigureDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureOptions<DbOptionsSetup>();

        #if DEBUG
            builder.Services.AddDbContext<DbContext, DevelopmentDbContext>();
        #else 
            builder.Services.AddDbContext<DbContext, ApplicationDbContext>();
        #endif

        return builder;
    }
}