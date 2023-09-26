using Mapingway.Application.Contracts.Auth;

namespace Mapingway.Application.Contracts.Abstractions.Authentication;

public interface IAccessTokenService
{
    public Task<AccessUnit> GenerateAccessToken(long userId, string email, CancellationToken? ct = null);
    string? GetEmailFromExpiredToken(string expiredToken);
}