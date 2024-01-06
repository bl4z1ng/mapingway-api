using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Mapingway.Infrastructure.Logging;

[ExcludeFromCodeCoverage]
public static class Configuration
{
    public static WebApplicationBuilder UseSerilog(
        this WebApplicationBuilder builder,
        bool clearProviders = true,
        params string[] propertiesToSkipLogEvent)
    {
        if (clearProviders)
        {
            // Remove additional logging providers - logs entry can be duplicated
            builder.Logging.ClearProviders();
        }

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);

            //Exclude infra paths, Swagger and health checks invokes
            foreach ( var path in propertiesToSkipLogEvent )
            {
                configuration.Filter
                    .ByExcluding(log => log.Properties.Any(p => p.ToString().Contains(path)));
            }
        });

        return builder;
    }
}
