namespace Mapingway.Common.Options;

public class DbOptions
{
    public string? ConnectionString { get; set; }
    public string? DefaultScheme { get; set; }
    public bool EnableLogging { get; set; }
    public bool EnableSensitiveDataLogging { get; set; }
    public bool EnableDetailedErrors { get; set; }
    
    public const string Development = "DevelopmentDatabaseConfiguration";
    public const string Production = "DatabaseConfiguration";
}