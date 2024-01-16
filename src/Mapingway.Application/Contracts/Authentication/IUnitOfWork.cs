using Mapingway.Domain.Auth;
using Mapingway.SharedKernel;

namespace Mapingway.Application.Contracts.Authentication;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IPermissionRepository Permissions { get; }
    IRepository<RefreshToken> RefreshTokens { get; }
    IRepository<RefreshTokenFamily> RefreshTokenFamilies { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
