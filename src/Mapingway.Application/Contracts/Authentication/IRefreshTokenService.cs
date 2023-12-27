using Mapingway.Domain.Auth;
using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Contracts.Authentication;

public interface IRefreshTokenService
{
    Task<Result<RefreshToken>> CreateTokenAsync(string email, CancellationToken ct = default);
    Task<Result<RefreshToken>> RefreshTokenAsync(string email, string oldTokenKey, CancellationToken ct = default);
    Task<Result> InvalidateTokenAsync(string email, string oldToken, CancellationToken ct = default);
}
