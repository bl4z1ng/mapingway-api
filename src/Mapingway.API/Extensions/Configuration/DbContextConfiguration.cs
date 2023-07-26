using Mapingway.Infrastructure.Persistence;
using Mapingway.Infrastructure.Persistence.Options;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.API.Extensions.Configuration;

public static class DbContextConfiguration
{
    public static WebApplicationBuilder ConfigureDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DbOptions>(builder.Configuration.GetSection(DbOptions.ConfigurationSection));

        if (builder.Environment.IsDevelopment()) 
            builder.Services.AddDbContext<DbContext, DevelopmentDbContext>();
        else 
            builder.Services.AddDbContext<DbContext, ApplicationDbContext>();

        return builder;
    }
}