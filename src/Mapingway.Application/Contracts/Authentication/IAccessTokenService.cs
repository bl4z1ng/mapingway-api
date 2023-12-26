using Mapingway.Application.Contracts.Authentication.Results;

namespace Mapingway.Application.Contracts.Authentication;

public interface IAccessTokenService
{
    public Task<AccessUnit> GenerateAccessToken(long userId, string email, CancellationToken? ct = null);
    string? GetEmailFromExpiredToken(string expiredToken);
}