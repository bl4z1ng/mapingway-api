﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Mapingway.Application.Abstractions;
using Mapingway.Common.Constants;
using Mapingway.Domain;
using Mapingway.Infrastructure.Authentication;
using Mapingway.Infrastructure.Authentication.Token;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;

namespace Mapingway.Infrastructure.Tests.Authentication;

public class AuthenticationServiceTests
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOptions<TokenValidationParameters> _tokenValidationParameters;
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly ITokenGenerator _tokenGenerator;

    public AuthenticationServiceTests()
    {
        _loggerFactory = Substitute.For<ILoggerFactory>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        var jwtOptions = new JwtOptions
        {
            Issuer = "Mapingway",
            Audience = "Mapingway",
            SigningKey = "Map",
            AccessTokenLifetime = new TimeSpan(0, 5, 0),
            RefreshTokenLifetime = new TimeSpan(24, 0, 0),
        };
        _jwtOptions = Substitute.For<IOptions<JwtOptions>>();
        _jwtOptions.Value
            .Returns(jwtOptions);
        
        _tokenValidationParameters = Substitute.For<IOptions<TokenValidationParameters>>();
        _tokenValidationParameters.Value
            .Returns(new TokenValidationParameters
            {
                ValidateLifetime = false,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                ValidAlgorithms = new List<string> { SecurityAlgorithms.HmacSha256 },
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
            });

        _tokenGenerator = Substitute.For<ITokenGenerator>();
    }

    private AuthenticationService Subject()
    {
        return new AuthenticationService(
            _loggerFactory,
            _jwtOptions,
            _tokenValidationParameters,
            _tokenGenerator,
            _unitOfWork);
    }
    
    [Fact]
    public async Task GenerateAccessToken_ValidUser_ValidToken()
    {
        // Arrange
        const string validAccessToken = "validAccessToken";
        var user = new User
        {
            Id = 1,
            Email = "user@rambler.ru",
            FirstName = "User",
            LastName = "User",
            PasswordHash = "123",
            PasswordSalt = "123"
        };
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };
        var permissions = new HashSet<string> { "ReadUser", "UpdateUser", "DeleteUser" };
        claims.AddRange(permissions.Select(p => new Claim(CustomClaimNames.Permissions, p)));
        var accessTokenDetails = new AccessTokenDetails
        (
            _jwtOptions.Value.Issuer,
            _jwtOptions.Value.Audience,
            _jwtOptions.Value.AccessTokenLifetime,
            Encoding.UTF8.GetBytes(_jwtOptions.Value.SigningKey),
            claims
        );
        var accessTokenDetails2 = new AccessTokenDetails
        (
            _jwtOptions.Value.Issuer,
            _jwtOptions.Value.Audience,
            _jwtOptions.Value.AccessTokenLifetime,
            Encoding.UTF8.GetBytes(_jwtOptions.Value.SigningKey),
            claims
        );

        var q = accessTokenDetails.Equals(accessTokenDetails2);
        _unitOfWork.Permissions.GetPermissionsAsync(1, CancellationToken.None).Returns(permissions);
        _tokenGenerator
            .GenerateAccessToken(Arg.Is<AccessTokenDetails>(a => a.Equals(accessTokenDetails)))
            .Returns(validAccessToken);
        var authenticationService = Subject();

        // Act
        var token = await authenticationService.GenerateAccessToken(user.Id, user.Email, CancellationToken.None);

        // Assert
        token.Should().NotBeNullOrWhiteSpace();
        _tokenGenerator.Received(1);
        token.Should().Be(validAccessToken);
    }
}