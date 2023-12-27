using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Mapingway.Application.Contracts;
using Mapingway.Domain;
using Mapingway.Infrastructure.Authentication;
using Mapingway.Infrastructure.Authentication.Claims;
using Mapingway.Infrastructure.Authentication.Token;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.Equivalency;

namespace Mapingway.Infrastructure.Tests.Authentication;

public class AuthenticationServiceTests
{
    private readonly IHasher _hasher;
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationServiceTests()
    {
        _loggerFactory = Substitute.For<ILoggerFactory>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _hasher = Substitute.For<IHasher>();

        var jwtOptions = new JwtOptions
        {
            Issuer = "Mapingway",
            Audience = "Mapingway",
            SigningKey = "Map",
            AccessTokenLifetime = new TimeSpan(0, 5, 0),
            RefreshTokenLifetime = new TimeSpan(24, 0, 0),
        };
        _jwtOptions = Substitute.For<IOptions<JwtOptions>>();
        _jwtOptions.Value.Returns(jwtOptions);

        _tokenGenerator = Substitute.For<ITokenGenerator>();
    }

    private AccessTokenService Subject()
    {
        return new AccessTokenService(
            _loggerFactory,
            _jwtOptions,
            _tokenGenerator,
            _hasher,
            _unitOfWork);
    }

    [Fact]
    public async Task GenerateAccessToken_ValidUser_ValidToken()
    {
        // Arrange
        const string validAccessToken = "validAccessToken";
        const string userContextToken = "123123123";
        const string userContextTokenHash = "ha$H";
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
        claims.AddRange(permissions.Select(p => new Claim(CustomClaims.Permissions, p)));
        claims.Add(new Claim(CustomClaims.UserContext, userContextTokenHash));
        var accessTokenDetails = new AccessTokenDetails
        (
            _jwtOptions.Value.Issuer,
            _jwtOptions.Value.Audience,
            _jwtOptions.Value.AccessTokenLifetime,
            Encoding.UTF8.GetBytes(_jwtOptions.Value.SigningKey),
            claims
        );

        _unitOfWork.Permissions
            .GetPermissionsAsync(1, CancellationToken.None)
            .Returns(permissions);
        _tokenGenerator
            .GenerateAccessToken(ArgEx.IsEquivalentTo(accessTokenDetails))
            .Returns(validAccessToken);
        _tokenGenerator
            .GenerateRandomToken()
            .Returns(userContextToken);
        _hasher
            .GenerateHash(userContextToken)
            .Returns(userContextTokenHash);
        var accessTokenService = Subject();

        // Act
        var token = await accessTokenService.GenerateAccessToken(user.Id, user.Email, CancellationToken.None);

        // Assert
        token.Value!.AccessToken.Should().NotBeNullOrWhiteSpace();
        token.Value!.AccessToken.Should().Be(validAccessToken);
        _tokenGenerator.Received(1);
    }
}
