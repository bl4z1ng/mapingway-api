using Serilog.Events;

namespace Mapingway.Infrastructure.Logging;

public class LoggingOptions
{
    public IEnumerable<string> IgnoredRequestPaths { get; set; } = Defaults.ExcludedRequestPaths;
    public string ServiceName { get; set; } = nameof(ServiceName);
    public string Version { get; set; } = nameof(Version);
    public bool ClearProviders { get; set; }
    public LogEventLevel MinimumLogLevel { get; set; } = LogEventLevel.Warning;
}

internal static class Defaults
{
    public static string LogMessageTemplate =>
        "Handled Request {RequestId:l} for {RequestMethod:l} {RequestScheme:l}://{RequestHost:l}{RequestPath:l} responded {StatusCode} in {Elapsed:0.0000} ms.";

    public static readonly string[] ExcludedRequestPaths =
        ["/favicon.ico", "/hc", "/health", "/swagger", "_vs/browserLink", "/_framework"];
}
