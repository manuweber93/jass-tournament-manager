using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassTournamentManager.Infrastructure.Persistence.Configurations
{
    public sealed class PairingParticipantConfiguration : IEntityTypeConfiguration<PairingParticipant>
    {
        public void Configure(EntityTypeBuilder<PairingParticipant> builder)
        {
            builder.ToTable("pairing_participants");

            builder.ConfigureBaseEntity();

            builder.Property(pairingParticipant => pairingParticipant.PairingId)
                .HasColumnName("pairing_id")
                .IsRequired();

            builder.Property(pairingParticipant => pairingParticipant.TournamentParticipantId)
                .HasColumnName("tournament_participant_id")
                .IsRequired();

            builder.HasOne<TournamentParticipant>()
                .WithMany()
                .HasForeignKey(pairingParticipant => pairingParticipant.TournamentParticipantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(pairingParticipant => pairingParticipant.Team)
                .HasColumnName("team")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(pairingParticipant => pairingParticipant.EnteredBy)
                .HasColumnName("entered_by")
                .IsRequired();

            builder.HasOne<TournamentParticipant>()
                .WithMany()
                .HasForeignKey(pairingParticipant => pairingParticipant.EnteredBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(pairingParticipant => pairingParticipant.PairingId)
                .HasDatabaseName("idx_pairing_participant_pairing");

            builder.HasIndex(pairingParticipant => pairingParticipant.TournamentParticipantId)
                .HasDatabaseName("idx_pairing_participant_participant");

            builder.HasIndex(pairingParticipant => new
            {
                pairingParticipant.PairingId,
                pairingParticipant.TournamentParticipantId
            }).IsUnique()
            .HasDatabaseName("ux_pairing_participants_pairing_tournament_participant");

        }
    }
}
