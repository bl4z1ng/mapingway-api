using Mapingway.Application.Contracts.Validation;

namespace Mapingway.Infrastructure.Validation.Name;

public class NameValidationRules : IValidationRules
{
    public static string? ConfigurationSection => "Name";

    public int MaxLength { get; init; }
}