namespace Mapingway.Application.Abstractions.Authentication;

public interface IPermissionRepository
{
    Task<HashSet<string>> GetPermissionsAsync(long userId, CancellationToken cancellationToken);
}