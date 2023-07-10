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
        
        // P4$$w0rd
        builder.HasData(new User
        {
            Id = -1,
            Email = "admin.map@rambler.ru",
            FirstName = "Admin",
            LastName = "Super",
            PasswordHash = "ODrNkGKssc+CWOvKQhJAQQNMocAsUaJ73pBaIfIufy4=",
            PasswordSalt = "u4ya35ZFIvfkqC+ObHlNFQ=="
        });
    }
}