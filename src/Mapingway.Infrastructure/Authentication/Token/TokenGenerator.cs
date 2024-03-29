﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Authentication.Token;

public class TokenGenerator : ITokenGenerator
{
    public string? GenerateAccessToken(AccessTokenDetails details)
    {
        if (details is not
            {
                Issuer.Length: > 0,
                Audience.Length: > 0,
                TokenLifeSpan.Ticks: > 0,
                SigningKeyBytes.Length: >= 16
            })
            return null;

        var signingKey = new SigningCredentials(
            new SymmetricSecurityKey(details.SigningKeyBytes), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            details.Issuer,
            details.Audience,
            details.Claims,
            DateTime.UtcNow,
            DateTime.UtcNow.Add(details.TokenLifeSpan),
            signingKey);

        var result = new JwtSecurityTokenHandler().WriteToken(token);

        return result;
    }

    public string GenerateRandomToken(int numberOfBytes = 16)
    {
        var rng = RandomNumberGenerator.Create();
        var bytes = new byte[numberOfBytes];
        rng.GetBytes(bytes);

        var value = Convert.ToBase64String(bytes);

        return value;
    }
}
