using System.Security.Claims;
using Mapingway.Domain;
using Mapingway.Domain.Auth;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IAuthenticationService
{
    string GenerateAccessToken(User user, IEnumerable<string> permissions);
    string GenerateRefreshToken();
    Task<RefreshToken?> RefreshTokenAsync(User user, string newRefreshToken, CancellationToken cancellationToken);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken);
}