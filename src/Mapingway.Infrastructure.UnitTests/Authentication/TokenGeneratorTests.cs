using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mapingway.Infrastructure.Authentication.Token;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Tests.Authentication;

public class TokenGeneratorTests
{
    [Fact]
    public void GenerateAccessToken_ValidInput_AccessTokenIsValid()
    {
        // arrange
        var generator = new TokenGenerator();
        const string issuer = "testIssuer";
        const string audience = "testAudience";
        var tokenLifespan = TimeSpan.FromHours(1);
        var signingKeyBytes = "signingKeyS1gningk3y123!"u8.ToArray();
        var claims = new List<Claim> { new(ClaimTypes.Name, "testUser") };

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };

        // act
        var accessToken = generator.GenerateAccessToken(
            issuer, 
            audience, 
            tokenLifespan, 
            signingKeyBytes, 
            claims);
        
        // assert
        Assert.NotNull(accessToken);
        Assert.NotEmpty(accessToken);
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var validatedToken);
        
        Assert.IsType<JwtSecurityToken>(validatedToken);
        var jwtToken = (JwtSecurityToken)validatedToken;

        Assert.NotNull(jwtToken);
        Assert.Equal(issuer, jwtToken.Issuer);
        Assert.Equal(audience, jwtToken.Audiences.First());
        Assert.True(jwtToken.ValidFrom <= DateTime.UtcNow && jwtToken.ValidTo >= DateTime.UtcNow);
        Assert.Equal("testUser", principal.Identity?.Name);
    }

    [Fact]
    public void GenerateAccessToken_ShortSigningKey_AccessTokenIsNull()
    {
        // arrange
        var generator = new TokenGenerator();
        const string issuer = "testIssuer";
        const string audience = "testAudience";
        var tokenLifespan = TimeSpan.FromHours(1);
        var signingKeyBytes = "123"u8.ToArray();
        var claims = new List<Claim> { new(ClaimTypes.Name, "testUser") };

        // act
        var accessToken = generator.GenerateAccessToken(
            issuer, 
            audience, 
            tokenLifespan, 
            signingKeyBytes, 
            claims);
        
        // assert
        Assert.Null(accessToken);
    }

    [Fact]
    public void GenerateAccessToken_EmptyIssuerOrAudience_AccessTokenIsNull()
    {
        // arrange
        var generator = new TokenGenerator();
        const string issuer = "";
        const string audience = "";
        var tokenLifespan = TimeSpan.FromHours(1);
        var signingKeyBytes = "1234567812345678"u8.ToArray();
        var claims = new List<Claim> { new(ClaimTypes.Name, "testUser") };

        // act
        var accessToken = generator.GenerateAccessToken(
            issuer, 
            audience, 
            tokenLifespan, 
            signingKeyBytes, 
            claims);
        
        // assert
        Assert.Null(accessToken);
    }

    [Fact]
    public void GenerateAccessToken_NegativeTimeSpan_AccessTokenIsNull()
    {
        // arrange
        var generator = new TokenGenerator();
        const string issuer = "testIssuer";
        const string audience = "testAudience";
        var tokenLifespan = TimeSpan.FromHours(-1);
        var signingKeyBytes = "1234567812345678"u8.ToArray();
        var claims = new List<Claim> { new(ClaimTypes.Name, "testUser") };

        // act
        var accessToken = generator.GenerateAccessToken(
            issuer, 
            audience, 
            tokenLifespan, 
            signingKeyBytes, 
            claims);
        
        // assert
        Assert.Null(accessToken);
    }

    [Fact]
    public void GenerateRefreshToken_Invoked_RefreshTokenIsValid()
    {
        // arrange
        var generator = new TokenGenerator();

        // act
        var refreshToken = generator.GenerateRefreshToken();

        // assert
        Assert.NotNull(refreshToken);
        Assert.NotEmpty(refreshToken);
    }
}