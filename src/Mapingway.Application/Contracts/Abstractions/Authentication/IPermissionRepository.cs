namespace Mapingway.Application.Contracts.Abstractions.Authentication;

public interface IPermissionRepository
{
    Task<HashSet<string>> GetPermissionsAsync(long userId, CancellationToken cancellationToken);
}