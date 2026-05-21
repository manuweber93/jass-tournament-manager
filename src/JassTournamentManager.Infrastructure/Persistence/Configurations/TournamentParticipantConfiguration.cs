using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassTournamentManager.Infrastructure.Persistence.Configurations
{
    public sealed class TournamentParticipantConfiguration : IEntityTypeConfiguration<TournamentParticipant>
    {
        public void Configure(EntityTypeBuilder<TournamentParticipant> builder)
        {
            builder.ToTable("tournament_participants");

            builder.ConfigureBaseEntity();

            builder.Property(tournamentParticipant => tournamentParticipant.TournamentId)
                .HasColumnName("tournament_id")
                .IsRequired();

            builder.Property(tournamentParticipant => tournamentParticipant.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(tournamentParticipant => tournamentParticipant.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(tournamentParticipant => tournamentParticipant.RegistrationMethod)
                .HasColumnName("registration_method")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(tournamentParticipant => tournamentParticipant.Role)
                .HasColumnName("role")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(tournamentParticipant => tournamentParticipant.IsPlaying)
                .HasColumnName("is_playing")
                .IsRequired();

            builder.HasIndex(tournamentParticipant => tournamentParticipant.TournamentId)
                .HasDatabaseName("idx_participant_tournament");

            builder.HasIndex(tournamentParticipant => tournamentParticipant.UserId)
                .HasDatabaseName("idx_participant_user");

            builder.HasIndex(tournamentParticipant => new
            {
                tournamentParticipant.UserId,
                tournamentParticipant.TournamentId,
            }).IsUnique()
            .HasDatabaseName("ux_tournament_participants_user_tournament");

        }
    }
}
