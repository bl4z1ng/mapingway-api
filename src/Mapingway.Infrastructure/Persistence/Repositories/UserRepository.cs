using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Domain;
using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }


    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await DbSet.FirstOrDefaultAsync(user => user.Email == email, ct);
    }

    public async Task<User?> GetByEmailWithPermissionsAsync(string email, CancellationToken ct)
    {
        return await DbSet
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.Email == email, ct);
    }
    
    public async Task<User?> GetByEmailWithRefreshTokensAsync(string email, CancellationToken ct)
    {
        return await DbSet
            .Include(user => user.RefreshToken)
            .Include(user => user.UsedRefreshTokensFamily)
            .FirstOrDefaultAsync(user => user.Email == email, ct);
    }

    public override async Task CreateAsync(User user, CancellationToken? ct = null)
    {
        user.UsedRefreshTokensFamily = new RefreshTokenFamily();

        await DbSet.AddAsync(user, ct ?? CancellationToken.None);

        foreach (var role in user.Roles)
        {
            Context.Entry(role).State = EntityState.Unchanged;
        }
    }
}