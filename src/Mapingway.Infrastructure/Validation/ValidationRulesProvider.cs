using System.Text.RegularExpressions;
using Mapingway.Application.Abstractions;

namespace Mapingway.Infrastructure.Validation;

public class ValidationRulesProvider : IValidationRulesProvider
{
    private readonly string _emailPattern;
    private readonly string _threeOrMoreLetters;


    public ValidationRulesProvider()
    {
        _emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        _threeOrMoreLetters = @"^(?:[a-zA-Z]){3,}$";
    }


    public bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, _emailPattern);
    }

    public bool Has3OrMoreLetters(string str)
    {
        return Regex.IsMatch(str, _threeOrMoreLetters);
    }
}