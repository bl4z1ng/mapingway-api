﻿using Mapingway.Common.Enums;
using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mapingway.Infrastructure.Persistence.Configuration;

public class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        
        builder.HasKey(permission => permission.Id);

        builder.Property(permission => permission.Name).IsRequired();

        var permissions = Enum.GetValues<Permissions>()
            .Select(permission => new Permission
            {
                Id = (int)permission,
                Name = permission.ToString()
            });

        builder.HasData(permissions);
    }
}