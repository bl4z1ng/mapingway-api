using System.Reflection;
using Mapingway.Infrastructure.Logging.Utility;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace Mapingway.Infrastructure.Logging
{
    public class LoggerBuilder
    {
        private readonly LoggerConfiguration _loggerConfiguration;
        private readonly LoggingOptions _loggingOptions;

        private LoggerBuilder(LoggingOptions loggingOptions, LoggerConfiguration loggerConfiguration)
        {
            _loggerConfiguration = loggerConfiguration;
            _loggingOptions = loggingOptions;
        }
        public static LoggerBuilder Create(LoggingOptions loggerOptions, LoggerConfiguration loggerConfiguration)
        {
            return new LoggerBuilder(loggerOptions, loggerConfiguration);
        }

        public LoggerBuilder WithAssembly(Assembly assembly)
        {
            _loggerConfiguration.Enrich.WithProperty(_loggingOptions.ServiceName, assembly.GetAssemblyName());
            _loggerConfiguration.Enrich.WithProperty(_loggingOptions.Version, assembly.GetAssemblyVersion());

            return this;
        }

        public LoggerBuilder WithDefaultConfiguration()
        {
            _loggerConfiguration.MinimumLevel.Is(_loggingOptions.MinimumLogLevel)
                .MinimumLevel.Override("System", _loggingOptions.MinimumLogLevel)
                .MinimumLevel.Override("Microsoft", _loggingOptions.MinimumLogLevel)
                .MinimumLevel.Override("Microsoft.AspNetCore", _loggingOptions.MinimumLogLevel)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", _loggingOptions.MinimumLogLevel)
                .MinimumLevel.Override("System.Net.Http.HttpClient", _loggingOptions.MinimumLogLevel)

                //TODO: move excluded routes to program.cs
                .Filter.ByExcluding(log =>
                {
                    return log.Properties.ContainsKey("Path")
                           && Array.Exists(Defaults.ExcludedRequestPaths,
                               s => log.Properties.TryGetValue("Path", out var value)
                                    && value.ToString().Contains(s, StringComparison.OrdinalIgnoreCase));
                })
                .Filter.ByExcluding(log => log.Properties.Any(p => p.ToString().Contains("health")))
                .Filter.ByExcluding(log => log.Properties.Any(p => p.ToString().Contains("index")))

                .Enrich.With<TraceActivityEventEnricher>();

            return this;
        }
    }
}
