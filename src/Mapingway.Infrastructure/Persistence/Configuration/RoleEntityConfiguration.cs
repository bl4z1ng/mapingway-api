using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mapingway.Infrastructure.Persistence.Configuration;

public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(role => role.Id);
        builder.HasAlternateKey(role => role.Name);

        builder
            .HasMany(role => role.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder
            .HasMany(role => role.Users)
            .WithMany()
            .UsingEntity<UserRole>();

        builder.HasData(Role.GetValues());
    }
}