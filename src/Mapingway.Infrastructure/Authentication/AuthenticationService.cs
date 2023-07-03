using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Application.Contracts.Token.Result;
using Mapingway.Common.Constants;
using Mapingway.Common.Exceptions;
using Mapingway.Domain;
using Mapingway.Domain.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Mapingway.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenGenerator _tokenGenerator;
    private readonly JwtOptions _jwtOptions;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(
        IOptions<JwtOptions> jwtOptions, 
        IOptions<TokenValidationParameters> tokenValidationOptions, 
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _tokenGenerator = tokenGenerator;
        _jwtOptions = jwtOptions.Value;

        _tokenValidationParameters = tokenValidationOptions.Value;
        _tokenValidationParameters.ValidateLifetime = false;

        _unitOfWork = unitOfWork;
        _refreshTokens = unitOfWork.RefreshTokens;
    }


    public string GenerateAccessToken(User user, IEnumerable<string> permissions)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!)
        };

        claims.AddRange(permissions.Select(p => new Claim(CustomClaimNames.Permissions, p)));

        var signingKey = Encoding.UTF8.GetBytes(_jwtOptions.SigningKey);

        var token = _tokenGenerator.GenerateAccessToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            _jwtOptions.AccessTokenLifetime,
            signingKey,
            claims);

        return token;
    }

    public string GenerateRefreshToken()
    {
        return _tokenGenerator.GenerateRefreshToken();
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken)
    {
        var tokenValidationHandler = new JwtSecurityTokenHandler();

        var principal = tokenValidationHandler.ValidateToken(
            expiredToken, 
            _tokenValidationParameters, 
            out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || 
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    public async Task<RefreshToken?> RefreshTokenAsync(User user, string newRefreshToken, CancellationToken cancellationToken)
    {
        if (user.RefreshToken is not null && user.RefreshToken.Value != newRefreshToken)
        {
            var tokenIsUsed = user.UsedRefreshTokens.Any(token => token.Value == newRefreshToken);
            if (tokenIsUsed)
            {
                //invalidate all tokens here
                //log here
                throw new RefreshTokenUsedException(newRefreshToken);
            }
            return null;
        }

        var refreshToken = await UpdateRefreshTokenAsync(user, newRefreshToken, cancellationToken);

        return refreshToken;
    }


    private async Task<RefreshToken?> UpdateRefreshTokenAsync(User user, string newToken, CancellationToken cancellationToken)
    {
        if (user.RefreshToken is null)
        {
            return null;
        }

        user.RefreshToken.IsUsed = true;
        user.UsedRefreshTokens.Add(user.RefreshToken);

        var refreshToken = RefreshTokenExtensions.CreateNotUsed(
            newToken,
            user.Id,
            _jwtOptions.RefreshTokenLifetime);

        user.RefreshToken = refreshToken; 

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }
}