namespace Mapingway.Application.Abstractions.Authentication;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsAsync(int userId);
}