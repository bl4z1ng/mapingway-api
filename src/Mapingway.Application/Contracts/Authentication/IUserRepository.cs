﻿using Mapingway.Domain;
using Mapingway.SharedKernel;

namespace Mapingway.Application.Contracts.Authentication;

public interface IUserRepository : IUserCheckRepository, IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<User?> GetByEmailWithPermissionsAsync(string email, CancellationToken ct = default);
    Task<User?> GetByIdWithRefreshTokensAsync(long id, CancellationToken ct = default);
    Task<User?> GetByEmailWithRefreshTokensAsync(string email, CancellationToken ct = default);
}
