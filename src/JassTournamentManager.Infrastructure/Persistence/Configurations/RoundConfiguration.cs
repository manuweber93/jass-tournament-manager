using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassTournamentManager.Infrastructure.Persistence.Configurations
{
    public sealed class RoundConfiguration : IEntityTypeConfiguration<Round>
    {
        public void Configure(EntityTypeBuilder<Round> builder)
        {
            builder.ToTable("rounds");

            builder.HasKey(round => round.Id);

            builder.Property(round => round.Id)
                .HasColumnName("id");

            builder.Property(round => round.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(round => round.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            builder.Property(round => round.TournamentId)
                .HasColumnName("tournament_id")
                .IsRequired();

            builder.Property(round => round.RoundNumber)
                .HasColumnName("round_number")
                .IsRequired();

            builder.Property(round => round.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.HasMany(round => round.Pairings)
                .WithOne()
                .HasForeignKey(pairing => pairing.RoundId);

            builder.Metadata
                .FindNavigation(nameof(Round.Pairings))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(round => round.TournamentId)
                .HasDatabaseName("idx_round_tournament");

            builder.HasIndex(round => new
            {
                round.TournamentId,
                round.RoundNumber
            })
                .IsUnique()
                .HasDatabaseName("ux_rounds_tournament_round_number");
        }
    }
}
