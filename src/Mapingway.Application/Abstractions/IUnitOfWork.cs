using Mapingway.Application.Abstractions.Authentication;

namespace Mapingway.Application.Abstractions;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IPermissionRepository Permissions { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IUsedRefreshTokenFamilyRepository UsedRefreshTokenFamilies { get; }


    Task SaveChangesAsync(CancellationToken cancellationToken);
}