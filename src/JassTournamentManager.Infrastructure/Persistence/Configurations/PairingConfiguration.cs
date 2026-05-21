using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassTournamentManager.Infrastructure.Persistence.Configurations
{
    public sealed class PairingConfiguration : IEntityTypeConfiguration<Pairing>
    {
        public void Configure(EntityTypeBuilder<Pairing> builder)
        {
            builder.ToTable("pairings");

            builder.ConfigureBaseEntity();

            builder.Property(pairing => pairing.RoundId)
                .HasColumnName("round_id")
                .IsRequired();

            builder.Property(pairing => pairing.JassTableId)
                .HasColumnName("jass_table_id")
                .IsRequired();

            builder.HasOne<JassTable>()
                .WithMany()
                .HasForeignKey(pairing => pairing.JassTableId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(pairing => pairing.GamesPerRound)
                .HasColumnName("games_per_round")
                .IsRequired();

            builder.Property(pairing => pairing.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.HasMany(pairing => pairing.Games)
                .WithOne()
                .HasForeignKey(game => game.PairingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata
                .FindNavigation(nameof(Pairing.Games))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(pairing => pairing.Participants)
                .WithOne()
                .HasForeignKey(pairingParticipant => pairingParticipant.PairingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata
                .FindNavigation(nameof(Pairing.Participants))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(pairing => pairing.RoundId)
                .HasDatabaseName("idx_pairing_round");

            builder.HasIndex(pairing => new
            {
                pairing.RoundId,
                pairing.JassTableId
            }).IsUnique()
            .HasDatabaseName("ux_pairings_round_jass_table");

        }
    }
}
