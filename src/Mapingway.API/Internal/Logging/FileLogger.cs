namespace Mapingway.API.Internal.Logging;

public class FileLogger : ILogger, IDisposable
{
    private readonly string _filePath;
    private readonly object _lock;


    public FileLogger(string filePath)
    {
        _filePath = filePath;
        _lock = new object();
    }


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        lock (_lock)
        {
            File.AppendAllText(_filePath, formatter(state, exception) + Environment.NewLine);
        }
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return this;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}