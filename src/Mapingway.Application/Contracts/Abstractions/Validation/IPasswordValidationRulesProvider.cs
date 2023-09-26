namespace Mapingway.Application.Contracts.Abstractions.Validation;

public interface IPasswordValidationRulesProvider
{
    int NumberOfLetters { get; }
    bool HasNOrMoreLetters(string str);
}