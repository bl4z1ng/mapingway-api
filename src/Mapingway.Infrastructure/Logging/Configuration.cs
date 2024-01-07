using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Mapingway.Infrastructure.Logging;

[ExcludeFromCodeCoverage]
public static class Configuration
{
    public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder, bool clearDefaultProviders = true)
    {
        // Remove default logging providers - logs entry can be duplicated
        if (clearDefaultProviders) builder.Logging.ClearProviders();

        builder.Host.UseSerilog((ctx, configuration) =>
            configuration.ReadFrom.Configuration(ctx.Configuration));

        // Used by enrichers to access request-scoped values
        builder.Services.AddHttpContextAccessor();

        return builder;
    }
}
