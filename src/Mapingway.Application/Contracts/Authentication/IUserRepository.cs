using Mapingway.SharedKernel;

namespace Mapingway.Application.Contracts.Authentication;

public interface IUserRepository : IUserCheckRepository, IRepository<Domain.User>
{
    Task<Domain.User?> GetByEmailAsync(string email, CancellationToken? ct = null);
    Task<Domain.User?> GetByEmailWithPermissionsAsync(string email, CancellationToken? ct = null);
    Task<Domain.User?> GetByIdWithRefreshTokensAsync(long id, CancellationToken? ct = null);
    Task<Domain.User?> GetByEmailWithRefreshTokensAsync(string email, CancellationToken? ct = null);
}