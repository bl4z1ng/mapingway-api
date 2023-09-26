using Mapingway.Application.Contracts.Abstractions.Authentication;

namespace Mapingway.Application.Contracts.Abstractions;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IPermissionRepository Permissions { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IUsedRefreshTokenFamilyRepository RefreshTokenFamilies { get; }


    Task SaveChangesAsync(CancellationToken cancellationToken);
}