using Mapingway.Application.Contracts.Authentication;
using Mapingway.Domain;
using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }


    public async Task<User?> GetByIdWithRefreshTokensAsync(long id, CancellationToken? ct = null)
    {
        return await DbSet
            .Include(user => user.RefreshTokensFamily)
            .ThenInclude(family => family.Tokens)
            .FirstOrDefaultAsync(user => user.Id == id, ct ?? CancellationToken.None);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken? ct = null)
    {
        return await DbSet.FirstOrDefaultAsync(user => user.Email == email, ct ?? CancellationToken.None);
    }

    public async Task<User?> GetByEmailWithPermissionsAsync(string email, CancellationToken? ct = null)
    {
        return await DbSet
            .Include(user => user.Roles)
            .FirstOrDefaultAsync(user => user.Email == email, ct ?? CancellationToken.None);
    }
    
    public async Task<User?> GetByEmailWithRefreshTokensAsync(string email, CancellationToken? ct = null)
    {
        return await DbSet
            .Include(user => user.RefreshTokensFamily)
            .ThenInclude(family => family.Tokens)
            .FirstOrDefaultAsync(user => user.Email == email, ct ?? CancellationToken.None);
    }

    public override async Task CreateAsync(User user, CancellationToken? ct = null)
    {
        user.RefreshTokensFamily = new RefreshTokenFamily();

        await DbSet.AddAsync(user, ct ?? CancellationToken.None);

        foreach (var role in user.Roles)
        {
            Context.Entry(role).State = EntityState.Unchanged;
        }
    }

    public Task<bool> DoesUserExistsByEmailAsync(string email, CancellationToken? ct = null)
    {
        return DbSet.AnyAsync(user => user.Email == email);
    }
}