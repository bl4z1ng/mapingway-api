using Mapingway.Domain.Auth;

namespace Mapingway.Application.Contracts.Abstractions.Authentication;

public interface IRefreshTokenService
{
    string GenerateRefreshToken();
    Task<RefreshToken?> UpdateRefreshTokenAsync(
        string email, 
        string newTokenValue, 
        string? oldTokenValue = null, 
        CancellationToken? cancellationToken = null);
    Task<bool> InvalidateRefreshToken(
        string email, 
        string refreshToken, 
        CancellationToken? ct = null);
}