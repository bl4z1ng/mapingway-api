using Mapingway.Application.Contracts.Authentication;

namespace Mapingway.Application.Contracts;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IPermissionRepository Permissions { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IUsedRefreshTokenFamilyRepository RefreshTokenFamilies { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
