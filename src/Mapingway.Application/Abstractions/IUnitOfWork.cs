using Mapingway.Application.Abstractions.Authentication;

namespace Mapingway.Application.Abstractions;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IPermissionRepository Permissions { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);
}