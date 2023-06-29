using System.Security.Claims;
using Mapingway.Domain.User;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IAuthenticationService
{
    string GenerateAccessToken(User user, IEnumerable<string> permissions);
    string GenerateRefreshToken();
    Task BindRefreshTokenToUserAsync(User user, string refreshToken, CancellationToken? cancellationToken = null);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken);
}