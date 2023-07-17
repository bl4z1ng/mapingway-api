namespace Mapingway.Infrastructure.Validation;

public class EmailValidationRules
{
    public const string ConfigurationSection = "Email";

    public string Pattern { get; init; } = null!;
}