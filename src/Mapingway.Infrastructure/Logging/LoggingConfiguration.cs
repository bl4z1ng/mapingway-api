using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;

namespace Mapingway.Infrastructure.Logging;

public static class LoggingConfiguration
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder,
        Type? exceptionMiddlewareType = default)
    {
        builder.UseSerilogRequestLogging(options =>
        {
            options.GetLevel = (httpContext, _, ex) =>
            {
                if ( ex is not null ) return LogEventLevel.Error;

                return httpContext.Response.StatusCode switch
                {
                    401 or 403 => LogEventLevel.Warning,
                    _ => LogEventLevel.Information
                };
            };

            //options.Enrich();
        });

        if ( exceptionMiddlewareType != default )
        {
            builder.UseMiddleware(exceptionMiddlewareType);
        }

        return builder;
    }

    private static void Enrich(this RequestLoggingOptions options)
    {
        //options.EnrichDiagnosticContext = DiagnosticContext.EnrichFromRequest;
        //options.MessageTemplate = DiagnosticContext.LogMessageTemplate;
    }
}
