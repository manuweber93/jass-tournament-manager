using JassTournamentManager.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassTournamentManager.Infrastructure.Persistence.Configurations
{
    internal static class EntityTypeBuilderExtensions
    {
        public static void ConfigureBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Id)
                .HasColumnName("id");

            builder.Property(entity => entity.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(entity => entity.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();
        }
    }
}
