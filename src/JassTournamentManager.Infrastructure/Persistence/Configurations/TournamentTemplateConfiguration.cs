using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassTournamentManager.Infrastructure.Persistence.Configurations
{
    public sealed class TournamentTemplateConfiguration : IEntityTypeConfiguration<TournamentTemplate>
    {
        public void Configure(EntityTypeBuilder<TournamentTemplate> builder)
        {
            builder.ToTable("tournament_templates");

            builder.ConfigureBaseEntity();

            builder.Property(tournamentTemplate => tournamentTemplate.OrganizerId)
                .HasColumnName("organizer_id")
                .IsRequired();

            builder.HasOne<User>()
                .WithOne()
                .HasForeignKey<TournamentTemplate>(tournamentTemplate => tournamentTemplate.OrganizerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.OwnsOne(tournamentTemplate => tournamentTemplate.ConfigValues, configBuilder =>
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

            builder.Navigation(tournamentTemplate => tournamentTemplate.ConfigValues)
                .IsRequired();

            builder.Property(tournamentTemplate => tournamentTemplate.Location)
                .HasColumnName("location")
                .HasMaxLength(200);

            builder.HasIndex(tournamentTemplate => tournamentTemplate.OrganizerId)
                .IsUnique()
                .HasDatabaseName("idx_tournament_template_organizer");
        }
    }
}
