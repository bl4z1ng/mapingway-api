using Mapingway.Domain;
using Mapingway.SharedKernel;

namespace Mapingway.Application.Contracts.Authentication;

public interface IUserRepository : IUserCheckRepository, IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken? ct = null);
    Task<User?> GetByEmailWithPermissionsAsync(string email, CancellationToken? ct = null);
    Task<User?> GetByIdWithRefreshTokensAsync(long id, CancellationToken? ct = null);
    Task<User?> GetByEmailWithRefreshTokensAsync(string email, CancellationToken? ct = null);
}