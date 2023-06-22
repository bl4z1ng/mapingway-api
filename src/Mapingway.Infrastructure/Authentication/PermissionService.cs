using Mapingway.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Authentication;

public class PermissionService : IPermissionService
{
    private readonly ApplicationDbContext _dbContext;


    public PermissionService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<HashSet<string>> GetPermissions(int userId)
    {
        var user = await _dbContext.Users
            .Include(user => user.Roles)
            .ThenInclude(role => role.Permissions)
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (user is null) return new HashSet<string>();

        return user.Roles
            .SelectMany(role => role.Permissions)
            .Select(permission => permission.Name)
            .ToHashSet();
    }
}