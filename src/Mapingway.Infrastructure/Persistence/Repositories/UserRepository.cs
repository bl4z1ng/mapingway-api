using Mapingway.Application.Abstractions;
using Mapingway.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;


    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<List<User>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Users.ToListAsync(ct);
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Users.FindAsync(new object?[] {id}, cancellationToken: ct);
    }

    public async Task<User?> GetByEmail(string email, CancellationToken ct = default)
    {
        return await _context.Users
            .Include(user => user.Roles)
            .ThenInclude(role => role.Permissions)
            .FirstOrDefaultAsync(user => user.Email == email, ct);
    }

    public async Task<List<User>> GetByConditionAsync(Func<User,bool> condition, CancellationToken ct = default)
    {
        return await _context.Users.Include(ent => condition).ToListAsync(ct);
    }

    public async Task<int> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken: cancellationToken);
        
        foreach (var role in user.Roles)
        {
            _context.Entry(role).State = EntityState.Unchanged;
        }
        
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var user = await _context.Users.FindAsync(new object?[] {id}, cancellationToken: ct);
        if (user is null) return false;

        _context.Entry(user).State = EntityState.Deleted;
        return true;
    }

    public void UpdateAsync(User user, CancellationToken ct = default)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}