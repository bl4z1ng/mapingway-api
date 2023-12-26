using Mapingway.Application.Contracts.Validation;

namespace Mapingway.Infrastructure.Validation.Email;

public class EmailValidationRules : IValidationRules
{
    public static string ConfigurationSection => "Email";

    public string Pattern { get; init; } = null!;
}