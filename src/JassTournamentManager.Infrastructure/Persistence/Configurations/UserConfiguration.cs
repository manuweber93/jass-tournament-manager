using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassTournamentManager.Infrastructure.Persistence.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.ConfigureBaseEntity();

            builder.Property(user => user.Email)
                .HasColumnName("email")
                .HasMaxLength(320);

            builder.Property(user => user.PasswordHash)
                .HasColumnName("password_hash");

            builder.Property(user => user.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(user => user.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(user => user.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            builder.Property(user => user.IsSysAdmin)
                .HasColumnName("is_sys_admin")
                .IsRequired();

            builder.Property(user => user.SourceType)
                .HasColumnName("source_type")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(user => user.MergeTargetUserId)
                .HasColumnName("merge_target_user_id");

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(user => user.MergeTargetUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(user => user.MergedBy)
                .HasColumnName("merged_by");

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(user => user.MergedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(user => user.MergedAt)
                .HasColumnName("merged_at");

            builder.HasIndex(user => user.MergeTargetUserId)
                .HasDatabaseName("idx_users_merge_target_user_id");

            builder.HasIndex(user => user.Email)
                .IsUnique()
                .AreNullsDistinct(true)
                .HasDatabaseName("ux_users_email");

        }
    }
}
