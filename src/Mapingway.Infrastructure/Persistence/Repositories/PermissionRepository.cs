using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Persistence.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly DbContext _dbContext;


    public PermissionRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<HashSet<string>> GetPermissionsAsync(int userId, CancellationToken cancellationToken)
    { 
        var user = await _dbContext.Set<User>()
            .Include(user => user.Roles)
            .ThenInclude(role => role.Permissions)
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

        if (user is null) return new HashSet<string>();

        return user.Roles
            .SelectMany(role => role.Permissions)
            .Select(permission => permission.Name)
            .ToHashSet();
    }
}