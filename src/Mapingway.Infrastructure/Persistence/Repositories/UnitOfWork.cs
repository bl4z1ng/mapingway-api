using Mapingway.Application.Abstractions;
using Mapingway.Application.Abstractions.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    public IUserRepository Users { get; }
    public IRefreshTokenRepository RefreshTokens { get; }
    public IPermissionRepository Permissions { get; }


    public UnitOfWork(
        DbContext context, 
        IUserRepository users, 
        IRefreshTokenRepository refreshTokens, 
        IPermissionRepository permissions)
    {
        _context = context;

        Users = users;
        RefreshTokens = refreshTokens;
        Permissions = permissions;
    }


    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}