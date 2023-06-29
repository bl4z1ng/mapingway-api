﻿using Mapingway.Application.Abstractions.Authentication;
using Mapingway.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace Mapingway.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DbContext context) : base(context)
    {
    }


    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await DbSet.FirstOrDefaultAsync(user => user.Email == email, ct);
    }

    public override async Task CreateAsync(User user, CancellationToken? ct = null)
    {
        await DbSet.AddAsync(user, ct ?? CancellationToken.None);
        
        foreach (var role in user.Roles)
        {
            Context.Entry(role).State = EntityState.Unchanged;
        }
    }
}