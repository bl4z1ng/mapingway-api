using Mapingway.Application.Contracts.Authentication;
using Mapingway.Domain.Auth;
using Mapingway.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;

    public UnitOfWork(
        DbContext context,
        IUserRepository users,
        IPermissionRepository permissions,
        IRepository<RefreshToken> refreshTokens,
        IRepository<RefreshTokenFamily> refreshTokenFamilies)
    {
        _context = context;

        Users = users;
        Permissions = permissions;
        RefreshTokens = refreshTokens;
        RefreshTokenFamilies = refreshTokenFamilies;
    }

    public IUserRepository Users { get; }
    public IPermissionRepository Permissions { get; }
    public IRepository<RefreshToken> RefreshTokens { get; }
    public IRepository<RefreshTokenFamily> RefreshTokenFamilies { get; }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
