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
        bool clearDefaultProviders = true,
        params string[] propertiesToSkipLogEvent)
    {
        // Remove default logging providers - logs entry can be duplicated
        if (clearDefaultProviders) builder.Logging.ClearProviders();

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);

            //Exclude infra paths, Swagger and health checks invokes
            foreach (var path in propertiesToSkipLogEvent)
            {
                configuration.Filter
                    .ByExcluding(log => log.Properties.Any(p => p.ToString().Contains(path)));
            }
        });

        return builder;
    }
}
