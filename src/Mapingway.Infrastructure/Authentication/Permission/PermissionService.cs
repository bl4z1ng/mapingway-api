using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Authentication.Permission;

public class PermissionService : IPermissionService
{
    private readonly ApplicationDbContext _dbContext;


    public PermissionService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<HashSet<string>> GetPermissionsAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
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