namespace Mapingway.Infrastructure.Validation.Email;

public class EmailValidationRules
{
    public const string ConfigurationSection = "Email";

    public string Pattern { get; init; } = null!;
}