namespace Mapingway.Application.Contracts.Authentication;

public interface IPermissionRepository
{
    Task<HashSet<string>> GetPermissionsAsync(long userId, CancellationToken cancellationToken);
}