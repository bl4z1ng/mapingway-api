namespace Mapingway.Application.Abstractions.Authentication;

public interface IPermissionRepository
{
    Task<HashSet<string>> GetPermissionsAsync(int userId, CancellationToken cancellationToken);
}