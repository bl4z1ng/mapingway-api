using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Authentication.Token;

public class TokenGenerator : ITokenGenerator
{
    public string GenerateAccessToken(
        string issuer, 
        string audience, 
        TimeSpan tokenLifespan, 
        byte[] signingKeyBytes, 
        IEnumerable<Claim> claims)
    {
        var signingKey = new SigningCredentials(
            new SymmetricSecurityKey(signingKeyBytes), 
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.Add(tokenLifespan),
            signingKey);

        var result = new JwtSecurityTokenHandler().WriteToken(token);

        return result;
    }

    public string GenerateRefreshToken()
    {
        var rng = RandomNumberGenerator.Create();
        var bytes = new byte[16];
        rng.GetBytes(bytes);

        var value = Convert.ToBase64String(bytes);

        return value;
    }
}