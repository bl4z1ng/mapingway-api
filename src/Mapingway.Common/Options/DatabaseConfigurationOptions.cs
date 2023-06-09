namespace Mapingway.Common.Options;

public class DatabaseConfigurationOptions
{
    public string ConnectionString { get; set; } = null!;
    public string DefaultScheme { get; set; } = null!;
    public bool EnableLogging { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }
    public bool EnableDetailedErrors { get; set; }
}