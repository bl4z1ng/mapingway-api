﻿using Mapingway.Domain;
using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mapingway.Infrastructure.Persistence.Configuration;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(user => user.Email)
            .IsRequired();

        builder
            .HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<UserRole>();
    }
}