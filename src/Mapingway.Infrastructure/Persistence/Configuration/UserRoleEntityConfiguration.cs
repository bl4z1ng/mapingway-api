using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mapingway.Infrastructure.Persistence.Configuration;

public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(x => new { x.UserId, x.RoleId });
        
        builder
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(b => b.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(b => b.Role)
            .WithMany(b => b.UserRoles)
            .HasForeignKey(b => b.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(p => p.UserId)
            .IsRequired();

        builder.HasData(Create(1, new List<Role> { Role.Admin }));
    }
    
    private static IEnumerable<UserRole> Create(long userId, IEnumerable<Role> roles)
    {
        return roles.Select(role => 
                new UserRole
                {
                    RoleId = role.Id, 
                    UserId = userId
                })
            .ToList();
    }
}