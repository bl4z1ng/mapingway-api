using Mapingway.Application.Contracts.Validation;
using Microsoft.Extensions.Options;

namespace Mapingway.Infrastructure.Validation.Name;

public class NameValidationRulesProvider : INameValidationRulesProvider
{
    public int MaxLength => _rules.MaxLength;

    private readonly NameValidationRules _rules;


    public NameValidationRulesProvider(IOptions<NameValidationRules> rules)
    {
        _rules = rules.Value;
    }
}