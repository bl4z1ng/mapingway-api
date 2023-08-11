using Mapingway.Application.Abstractions;
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
        _jwtOptions = Substitute.For<IOptions<JwtOptions>>();
        _tokenValidationParameters = Substitute.For<IOptions<TokenValidationParameters>>();
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
        var user = new User
        {
            Id = 1,
            Email = "user@rambler.ru",
            FirstName = "User",
            LastName = "User",
            PasswordHash = "123",
            PasswordSalt = "123"
        };
        _unitOfWork.Permissions.GetPermissionsAsync(1, CancellationToken.None)
            .Returns(new HashSet<string> { "ReadUser", "UpdateUser", "DeleteUser" });
        
        _jwtOptions.Value
            .Returns(new JwtOptions
            {
                Issuer = "Mapingway",
                Audience = "Mapingway",
                SigningKey = "Map",
                AccessTokenLifetime = new TimeSpan(0, 5, 0),
                RefreshTokenLifetime = new TimeSpan(360, 0, 0),
            });
        var authenticationService = Subject();

        var token = await authenticationService.GenerateAccessToken(user.Id, user.Email, CancellationToken.None);
        
        Assert.NotNull(token);
    }
}