using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassTournamentManager.Infrastructure.Persistence.Configurations
{
    public sealed class JassTableConfiguration : IEntityTypeConfiguration<JassTable>
    {
        public void Configure(EntityTypeBuilder<JassTable> builder)
        {
            builder.ToTable("jass_tables");

            builder.ConfigureBaseEntity();

            builder.Property(jassTable => jassTable.OrganizerId)
                .HasColumnName("organizer_id")
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(jassTable => jassTable.OrganizerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(jassTable => jassTable.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(jassTable => jassTable.DisplayOrder)
                .HasColumnName("display_order")
                .IsRequired();

            builder.Property(jassTable => jassTable.IsActive)
                .HasColumnName("is_active")
                .IsRequired();

            builder.HasIndex(jassTable => jassTable.OrganizerId)
                .HasDatabaseName("idx_table_organizer");

            builder.HasIndex(jassTable => new
            {
                jassTable.OrganizerId,
                jassTable.IsActive
            }).HasDatabaseName("idx_table_active");

        }
    }
}
