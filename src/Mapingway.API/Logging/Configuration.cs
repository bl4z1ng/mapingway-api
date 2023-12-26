using Serilog;

namespace Mapingway.API.Logging;

public static class Configuration
{
    //TODO: iterate, add ok structured logging template
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        return builder;
    }
}