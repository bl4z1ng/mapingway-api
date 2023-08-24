using System.Text.RegularExpressions;
using Mapingway.Application.Abstractions.Validation;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Validation.Password;

public class PasswordValidationRulesProvider : IPasswordValidationRulesProvider
{
    private readonly PasswordValidationRules _passwordValidationRules;


    public int NumberOfLetters => _passwordValidationRules.NumberOfLetters;


    public PasswordValidationRulesProvider(IOptions<PasswordValidationRules> passwordRules)
    {
        _passwordValidationRules = passwordRules.Value;
    }


    public bool HasNOrMoreLetters(string password)
    {
        return Regex.IsMatch(password, _passwordValidationRules.NOrMoreLettersPattern);
    }
}