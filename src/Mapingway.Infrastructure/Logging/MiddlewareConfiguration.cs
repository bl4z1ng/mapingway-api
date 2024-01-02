using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace Mapingway.Infrastructure.Logging;

public static class MiddlewareConfiguration
{
    public static void UseRequestLoggingWith<T>(this IApplicationBuilder builder)
    {
        builder.UseSerilogRequestLogging(options =>
        {
            options.GetLevel = (httpContext, _, ex) => ex != null
                ? LogEventLevel.Error
                : httpContext.Response.StatusCode is 401 or 403
                    ? LogEventLevel.Warning
                    : LogEventLevel.Information;

            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                var request = httpContext.Request;

                diagnosticContext.Set("RequestHost", request.Host.Value);
                diagnosticContext.Set("RequestScheme", request.Scheme);
                diagnosticContext.Set("Protocol", request.Protocol);
                diagnosticContext.Set("TraceId", httpContext.TraceIdentifier);
                diagnosticContext.Set("ContentType", request.ContentType);
            };
        }).UseMiddleware<T>();
    }
}
