using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Mapingway.Infrastructure.Authentication.Token;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Tests.Authentication;

public class TokenGeneratorTests
{
    private static readonly AccessTokenDetails _validAccessTokenData;
    private static readonly AccessTokenDetails _invalidAccessTokenData;

    static TokenGeneratorTests()
    {
        _validAccessTokenData = new AccessTokenDetails(
            Issuer: "testIssuer",
            Audience: "testAudience",
            TokenLifeSpan: TimeSpan.FromHours(1),
            SigningKeyBytes: "signingKeyS1gningk3y123qweqweqweqweqwe!"u8.ToArray(),
            Claims: new List<Claim> { new(ClaimTypes.Name, "testUser") });

        _invalidAccessTokenData = new AccessTokenDetails(
            Issuer: string.Empty,
            Audience: string.Empty,
            TokenLifeSpan: TimeSpan.FromHours(-1),
            SigningKeyBytes: ""u8.ToArray(),
            Claims: new List<Claim> { new(ClaimTypes.Name, "testUser") });
    }

    private static TokenGenerator Subject()
    {
        return new TokenGenerator();
    }

    public static List<object?[]> GenerateInvalidAccessTokenData()
    {
        return
        [
            [_validAccessTokenData with { Audience = _invalidAccessTokenData.Audience }, null],
            [_validAccessTokenData with { Issuer = _invalidAccessTokenData.Issuer }, null],
            [_validAccessTokenData with { SigningKeyBytes = _invalidAccessTokenData.SigningKeyBytes }, null],
            [_validAccessTokenData with { TokenLifeSpan = _invalidAccessTokenData.TokenLifeSpan }, null]
        ];
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
            ValidIssuer = _validAccessTokenData.Issuer,
            ValidateAudience = true,
            ValidAudience = _validAccessTokenData.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(_validAccessTokenData.SigningKeyBytes)
        };

        // act
        var accessToken = generator.GenerateAccessToken(_validAccessTokenData);

        // assert
        accessToken.Should().NotBeNullOrEmpty();

        var exception = Record.Exception(
            () => tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out _));
        exception.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(GenerateInvalidAccessTokenData))]
    public void GenerateAccessToken_InvalidDetails_AccessTokenIsNull(AccessTokenDetails details, string? expected)
    {
        var generator = Subject();

        var accessToken = generator.GenerateAccessToken(details);

        accessToken.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GenerateRandomToken_Valid_RefreshTokenIsValid()
    {
        var generator = new TokenGenerator();

        var refreshToken = generator.GenerateRandomToken();

        refreshToken.Should().NotBeNullOrEmpty();
    }
}
