using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassTournamentManager.Infrastructure.Persistence.Configurations
{
    public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("refresh_tokens");

            builder.ConfigureBaseEntity();

            builder.Property(refreshToken => refreshToken.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(refreshToken => refreshToken.TokenHash)
                .HasColumnName("token_hash")
                .IsRequired();

            builder.Property(refreshToken => refreshToken.ExpiresAtUtc)
                .HasColumnName("expires_at_utc")
                .IsRequired();

            builder.Property(refreshToken => refreshToken.RevokedAtUtc)
                .HasColumnName("revoked_at_utc");

            builder.Property(refreshToken => refreshToken.ReplacedByTokenId)
                .HasColumnName("replaced_by_token_id");

            builder.HasIndex(refreshToken => refreshToken.UserId)
                .HasDatabaseName("idx_refresh_token_user_id");

            builder.HasIndex(refreshToken => refreshToken.TokenHash)
                .IsUnique()
                .HasDatabaseName("ux_refresh_token_token_hash");

            builder.HasIndex(refreshToken => refreshToken.ExpiresAtUtc)
                .HasDatabaseName("idx_refresh_token_expires_at_utc");
        }
    }
}
