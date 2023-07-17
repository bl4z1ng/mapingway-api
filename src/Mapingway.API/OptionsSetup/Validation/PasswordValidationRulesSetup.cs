using Mapingway.Infrastructure.Validation;
using Microsoft.Extensions.Options;

namespace Mapingway.API.OptionsSetup.Validation;

public class PasswordValidationRulesSetup : IConfigureOptions<PasswordValidationRules>
{
    private readonly IConfiguration _configuration;


    public PasswordValidationRulesSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public void Configure(PasswordValidationRules options)
    {
        var path = $"{ValidationOptions.ConfigurationSection}:{PasswordValidationRules.ConfigurationSection}";

        _configuration.GetSection(path).Bind(options);
    }
}