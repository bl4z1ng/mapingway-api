using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace Mapingway.Infrastructure.Logging;

[ExcludeFromCodeCoverage]
public static class MiddlewareConfiguration
{
    public const string LogMessageTemplate =
        "{RequestMethod:l} Request {RequestId:l} to {RequestScheme:l}://{RequestHost:l}{RequestPath:l} finished with {StatusCode} in {Elapsed:0.0000} ms.";

    public static void UseRequestLoggingWith<T>(this IApplicationBuilder builder)
    {
        builder.UseSerilogRequestLogging(options =>
        {
            options.GetLevel = (httpContext, _, ex) =>
            {
                if (ex != null) return LogEventLevel.Error;

                return httpContext.Response.StatusCode switch
                {
                    401 or 403 => LogEventLevel.Warning,
                    _ => LogEventLevel.Information
                };
            };

            options.IncludeQueryInRequestPath = true;
            options.MessageTemplate = LogMessageTemplate;

            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                var request = httpContext.Request;

                diagnosticContext.Set("RequestHost", request.Host.Value);
                diagnosticContext.Set("RequestScheme", request.Scheme);
                diagnosticContext.Set("Protocol", request.Protocol);
                diagnosticContext.Set("TraceId", httpContext.TraceIdentifier);
                diagnosticContext.Set("ContentType", request.ContentType);
                diagnosticContext.Set("SpanId", Activity.Current?.SpanId);
            };
        }).UseMiddleware<T>();
    }
}
