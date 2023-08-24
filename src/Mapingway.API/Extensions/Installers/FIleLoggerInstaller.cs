using Mapingway.API.Internal.Logging;

namespace Mapingway.API.Extensions.Installers;

public static class FIleLoggerInstaller
{
    public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, string filePath)
    {
        return builder.AddProvider(new FileLoggerProvider(filePath));
    }
}