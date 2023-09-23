using Mapingway.Application.Contracts;
using Mapingway.Domain;
using Mapingway.Domain.Auth;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IAuthenticationService : IAccessTokenParser
{
    Task<AccessUnit> GenerateAccessToken(long userId, string email, CancellationToken? cancellationToken = null);
    string GenerateRefreshToken();
    Task<RefreshToken?> RefreshTokenAsync(
        User user, 
        string newRefreshToken, 
        string? oldRefreshToken = null, 
        CancellationToken? cancellationToken = null);
    public bool InvalidateRefreshToken(User user);
}