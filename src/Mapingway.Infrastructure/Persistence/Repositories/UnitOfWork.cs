using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    public IUserRepository Users { get; }
    public IPermissionRepository Permissions { get; }
    public IRefreshTokenRepository RefreshTokens { get; }
    public IUsedRefreshTokenFamilyRepository RefreshTokenFamilies { get; }


    public UnitOfWork(
        DbContext context, 
        IUserRepository users, 
        IPermissionRepository permissions,
        IRefreshTokenRepository refreshTokens,
        IUsedRefreshTokenFamilyRepository usedRefreshTokenFamilies)
    {
        _context = context;

        Users = users;
        Permissions = permissions;
        RefreshTokens = refreshTokens;
        RefreshTokenFamilies = usedRefreshTokenFamilies;
    }


    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}