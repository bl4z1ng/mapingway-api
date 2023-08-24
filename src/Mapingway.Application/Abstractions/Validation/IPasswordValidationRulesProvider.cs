namespace Mapingway.Application.Abstractions.Validation;

public interface IPasswordValidationRulesProvider
{
    int NumberOfLetters { get; }
    bool HasNOrMoreLetters(string str);
}