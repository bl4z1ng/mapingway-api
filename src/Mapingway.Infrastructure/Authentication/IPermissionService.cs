namespace Mapingway.Infrastructure.Authentication;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissions(int userId);
}