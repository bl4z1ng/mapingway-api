namespace Mapingway.API.Internal.Logging;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _filePath;


    public FileLoggerProvider(string filePath)
    {
        _filePath = filePath;
    }


    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(_filePath);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}