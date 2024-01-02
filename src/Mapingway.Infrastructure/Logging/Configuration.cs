using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Mapingway.Infrastructure.Logging;

public static class Configuration
{
    public static IHostBuilder UseSerilog<T>(this IHostBuilder builder,
        Action<LoggingOptions>? bindOptions = default)
    {
        var options = new LoggingOptions();
        bindOptions?.Invoke(options);

        // Remove additional logging providers - logs entry can be duplicated
        if (options.ClearProviders)
            builder.ConfigureLogging(p => p.ClearProviders());

        builder.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        //builder.UseSerilog((ctx, _, configuration) =>
        //{
        //    LoggerBuilder.Create(options, configuration)
        //        .WithDefaultConfiguration()
        //        .WithAssembly(typeof(T).Assembly);
//
        //}, writeToProviders: true);

        return builder;
    }
}
