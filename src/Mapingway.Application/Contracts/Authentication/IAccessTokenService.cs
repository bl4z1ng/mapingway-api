using Mapingway.SharedKernel.Result;

namespace Mapingway.Application.Contracts.Authentication;

public interface IAccessTokenService
{
    public Task<Result<AccessUnit>> GenerateAccessToken(long userId, string email, CancellationToken ct = default);
}
