using Mapingway.Domain.User;
using Mapingway.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Repositories;

public class UserRepository : IRepository<User>
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
        return await _context.Users.FindAsync(id, ct);
    }

    public async Task<List<User>> GetByConditionAsync(Func<User,bool> condition, CancellationToken ct = default)
    {
        return await _context.Users.Include(ent => condition).ToListAsync(ct);
    }

    public async Task<int> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken: cancellationToken);
        _context.Entry(user).State = EntityState.Added;

        return user.Id;
    }

    public async Task<bool> DeleteAsync(int userId, CancellationToken ct = default)
    {
        var user = await _context.Users.FindAsync(userId, ct);
        if (user is null) return false;

        _context.Entry(user).State = EntityState.Deleted;
        return true;
    }

    public void UpdateAsync(User user, CancellationToken ct = default)
    {
        _context.Entry(user).State = EntityState.Modified;
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}