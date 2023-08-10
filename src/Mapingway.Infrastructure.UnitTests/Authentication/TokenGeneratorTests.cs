using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Mapingway.Infrastructure.Authentication.Token;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Tests.Authentication;

public class TokenGeneratorTests
{
    private static AccessTokenDetails ValidAccessTokenData => new(
        Issuer: "testIssuer",
        Audience: "testAudience",
        TokenLifeSpan: TimeSpan.FromHours(1),
        SigningKeyBytes: "signingKeyS1gningk3y123!"u8.ToArray(),
        Claims: new List<Claim> { new(ClaimTypes.Name, "testUser") });

    private static AccessTokenDetails InvalidAccessTokenData => new (
        Issuer: string.Empty,
        Audience: string.Empty, 
        TokenLifeSpan: TimeSpan.FromHours(-1),
        SigningKeyBytes: ""u8.ToArray(),
        Claims: new List<Claim> { new(ClaimTypes.Name, "testUser") });


    private static TokenGenerator Subject()
    {
        return new TokenGenerator();
    }

    public static List<object?[]> GenerateInvalidAccessTokenData()
    {
        return new List<object?[]>
        {
            new object?[] { ValidAccessTokenData with { Audience = InvalidAccessTokenData.Audience }, null },
            new object?[] { ValidAccessTokenData with { Issuer = InvalidAccessTokenData.Issuer }, null },
            new object?[]
                { ValidAccessTokenData with { SigningKeyBytes = InvalidAccessTokenData.SigningKeyBytes }, null },
            new object?[]
                { ValidAccessTokenData with { TokenLifeSpan = InvalidAccessTokenData.TokenLifeSpan }, null },
        };
    }

    [Fact]
    public void GenerateAccessToken_ValidDetails_AccessTokenIsValid()
    {
        // arrange
        var generator = Subject();
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = ValidAccessTokenData.Issuer,
            ValidateAudience = true,
            ValidAudience = ValidAccessTokenData.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(ValidAccessTokenData.SigningKeyBytes)
        };

        // act
        var accessToken = generator.GenerateAccessToken(ValidAccessTokenData);

        // assert
        Assert.NotNull(accessToken);
        Assert.NotEmpty(accessToken);

        var exception = Record.Exception(
            () => tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out _));
        Assert.Null(exception);
    }

    [Theory]
    [MemberData(nameof(GenerateInvalidAccessTokenData))]
    public void GenerateAccessToken_InvalidDetails_AccessTokenIsNull(AccessTokenDetails details, string? expected)
    {
        var generator = Subject();

        var accessToken = generator.GenerateAccessToken(details);

        Assert.Equal(accessToken, expected);
    }

    [Fact]
    public void GenerateRefreshToken_Valid_RefreshTokenIsValid()
    {
        var generator = new TokenGenerator();

        var refreshToken = generator.GenerateRefreshToken();

        Assert.NotNull(refreshToken);
        Assert.NotEmpty(refreshToken);
    }
}