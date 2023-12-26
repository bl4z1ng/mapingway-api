using Mapingway.Domain.Auth;

namespace Mapingway.Application.Contracts.Authentication;

public interface IRefreshTokenService
{
    Task<RefreshToken?> CreateTokenAsync(
        string email, 
        CancellationToken? cancellationToken = null);
    Task<RefreshToken?> RefreshTokenAsync(
        string email, 
        string oldTokenKey, 
        CancellationToken? cancellationToken = null);
    Task<bool> InvalidateTokenAsync(
        string email, 
        string oldTokenKey, 
        CancellationToken? ct = null);
}