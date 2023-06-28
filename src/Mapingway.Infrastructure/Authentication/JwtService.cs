using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Application.Contracts.User.Result;
using Mapingway.Common.Constants;
using Mapingway.Common.Repository;
using Mapingway.Domain.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Authentication;

public class JwtService : IJwtService
{
    private readonly IRepository<RefreshToken> _tokenRepository;
    private readonly JwtOptions _options;

    public JwtService(IOptions<JwtOptions> options, IRepository<RefreshToken> tokenRepository)
    {
        _tokenRepository = tokenRepository;
        _options = options.Value;
    }


    public async Task<AuthenticationResult> GenerateTokensAsync(
        User user, 
        IEnumerable<string> permissions, 
        CancellationToken cancellationToken)
    {
        var token = GenerateAccessToken(user, permissions);
        var refreshTokenValue = GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Value = refreshTokenValue,
            User = user,
            IsUsed = false,
            ExpiresAt = DateTime.UtcNow.Add(_options.RefreshTokenLifetime)
        };

        await _tokenRepository.CreateAsync(refreshToken, cancellationToken);

        return new AuthenticationResult
        {
            Token = token,
            RefreshToken = refreshTokenValue
        };
    }
    
    private string GenerateAccessToken(User user, IEnumerable<string> permissions)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        };

        claims.AddRange(permissions.Select(p => new Claim(CustomClaimNames.Permissions, p)));

        var signingKey = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey)), 
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.Add(_options.AccessTokenLifetime),
            signingKey);

        var result = new JwtSecurityTokenHandler().WriteToken(token);

        return result;
    }
    private string GenerateRefreshToken()
    {
        var rng = RandomNumberGenerator.Create();
        var bytes = new byte[16];
        
        rng.GetBytes(bytes);

        var token = Convert.ToBase64String(bytes);

        return token;
    }
}