using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassTournamentManager.Infrastructure.Persistence.Configurations
{
    public sealed class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
    {
        public void Configure(EntityTypeBuilder<Tournament> builder)
        {
            builder.ToTable("tournaments");

            builder.HasKey(tournament => tournament.Id);

            builder.Property(tournament => tournament.Id)
                .HasColumnName("id");

            builder.Property(tournament => tournament.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(tournament => tournament.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            builder.Property(tournament => tournament.OrganizerId)
                .HasColumnName("organizer_id")
                .IsRequired();

            builder.Property(tournament => tournament.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.OwnsOne(tournament => tournament.Details, detailsBuilder =>
            {
                detailsBuilder.Property(details => details.Name)
                    .HasColumnName("name")
                    .HasMaxLength(200)
                    .IsRequired();

                detailsBuilder.Property(details => details.Location)
                    .HasColumnName("location")
                    .HasMaxLength(200);

                detailsBuilder.Property(details => details.Date)
                    .HasColumnName("date")
                    .IsRequired();

                detailsBuilder.Property(details => details.TournamentCode)
                    .HasColumnName("tournament_code")
                    .HasMaxLength(20)
                    .IsRequired();

                detailsBuilder.HasIndex(details => details.TournamentCode)
                    .IsUnique()
                    .HasDatabaseName("ux_tournaments_tournament_code");
            });

            builder.OwnsOne(tournament => tournament.ConfigValues, configBuilder =>
            {
                configBuilder.Property(config => config.NumberOfRounds)
                    .HasColumnName("number_of_rounds")
                    .IsRequired();

                configBuilder.Property(config => config.GamesPerRound)
                    .HasColumnName("games_per_round")
                    .IsRequired();

                configBuilder.Property(config => config.MatchBonusEnabled)
                    .HasColumnName("match_bonus_enabled")
                    .IsRequired();

                configBuilder.Property(config => config.IsFixedTeams)
                    .HasColumnName("is_fixed_teams")
                    .IsRequired();

                configBuilder.Property(config => config.ScoreVisibility)
                    .HasColumnName("score_visibility")
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .IsRequired();
            });

            builder.Navigation(tournament => tournament.Details)
                .IsRequired();

            builder.Navigation(tournament => tournament.ConfigValues)
                .IsRequired();

            builder.HasMany(tournament => tournament.Rounds)
                .WithOne()
                .HasForeignKey(round => round.TournamentId);

            builder.Metadata
                .FindNavigation(nameof(Tournament.Rounds))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(tournament => tournament.Participants)
                .WithOne()
                .HasForeignKey(participant => participant.TournamentId);

            builder.Metadata
                .FindNavigation(nameof(Tournament.Participants))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(tournament => tournament.OrganizerId)
                .HasDatabaseName("idx_tournament_organizer");

            builder.HasIndex(tournament => tournament.Status)
                .HasDatabaseName("idx_tournament_status");
        }
    }
}
