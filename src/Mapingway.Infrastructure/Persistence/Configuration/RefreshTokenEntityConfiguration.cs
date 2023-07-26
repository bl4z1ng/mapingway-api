using Mapingway.Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mapingway.Infrastructure.Persistence.Configuration;

public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");
        builder
            .HasKey(token => token.Id);
        builder
            .Property(token => token.Id)
            .UseIdentityColumn();

        builder
            .HasAlternateKey(token => token.Value);
        
        builder
            .HasOne(token => token.User)
            .WithOne(user => user.RefreshToken)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}