using Mapingway.Common.Repository;
using Mapingway.Domain.User;

namespace Mapingway.Application.Abstractions;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmail(string email, CancellationToken ct);
}