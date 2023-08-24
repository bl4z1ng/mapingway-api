using Mapingway.Common.Interfaces;
using Mapingway.Domain;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<User?> GetByEmailWithPermissionsAsync(string email, CancellationToken ct);
    Task<User?> GetByEmailWithRefreshTokensAsync(string email, CancellationToken ct);
}