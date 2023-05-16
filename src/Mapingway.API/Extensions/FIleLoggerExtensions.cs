using Mapingway.API.Internal.Logging;

namespace Mapingway.API.Extensions;

public static class FIleLoggerExtensions
{
    public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, string filePath)
    {
        return builder.AddProvider(new FileLoggerProvider(filePath));
    }
}