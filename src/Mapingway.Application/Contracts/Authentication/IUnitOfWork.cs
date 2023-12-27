namespace Mapingway.Application.Contracts.Authentication;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IPermissionRepository Permissions { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IUsedRefreshTokenFamilyRepository RefreshTokenFamilies { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
