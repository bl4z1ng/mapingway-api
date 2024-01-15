using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Authentication.Token.Parser;

public class JwtTokenParser : IJwtTokenParser
{
    private readonly TokenValidationParameters _expiredTokenValidationParameters;

    public JwtTokenParser(IOptions<TokenValidationParameters> validationParameters)
    {
        ArgumentNullException.ThrowIfNull(validationParameters.Value);

        _expiredTokenValidationParameters = validationParameters.Value.Clone();
        _expiredTokenValidationParameters.ValidateLifetime = false;
    }

    public ClaimsPrincipal GetPrincipalFromBearer(string token)
    {
        var tokenValidationHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenValidationHandler.ValidateToken(
                token,
                _expiredTokenValidationParameters,
                out var securityToken);

            if (securityToken is not JwtSecurityToken)
                throw new ArgumentException("Provided token is not valid JWT Bearer.");

            return principal;
        }
        catch (ArgumentException e)
        {
            throw new SecurityTokenException(message: "Recieved token is not valid.", innerException: e);
        }
    }
}
