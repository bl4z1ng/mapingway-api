using System.Text.RegularExpressions;
using Mapingway.Application.Abstractions;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Validation;

public class ValidationRulesProvider : IValidationRulesProvider
{
    private readonly PasswordValidationRules _passwordRules;
    private readonly EmailValidationRules _emailRules;

    public ValidationRulesProvider(
        IOptions<PasswordValidationRules> passwordRules, 
        IOptions<EmailValidationRules> emailRules)
    {
        _passwordRules = passwordRules.Value;
        _emailRules = emailRules.Value;
    }


    public bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, _emailRules.Pattern);
    }

    public int PasswordNumberOfLetters => _passwordRules.NumberOfLetters;
    public bool HasNOrMoreLetters(string str)
    {
        return Regex.IsMatch(str, _passwordRules.NOrMoreLettersPattern);
    }
}