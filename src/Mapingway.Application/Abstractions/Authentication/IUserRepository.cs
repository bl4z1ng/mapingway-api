using Mapingway.Common;
using Mapingway.Domain;

namespace Mapingway.Application.Abstractions.Authentication;

public interface IUserRepository : IUserCheckRepository, IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken? ct = null);
    Task<User?> GetByEmailWithPermissionsAsync(string email, CancellationToken? ct = null);
    Task<User?> GetByIdWithRefreshTokensAsync(long id, CancellationToken? ct = null);
    Task<User?> GetByEmailWithRefreshTokensAsync(string email, CancellationToken? ct = null);
}