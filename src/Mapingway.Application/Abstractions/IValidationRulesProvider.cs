namespace Mapingway.Application.Abstractions;

public interface IValidationRulesProvider
{
    bool IsValidEmail(string email);
    bool Has3OrMoreLetters(string str);
}