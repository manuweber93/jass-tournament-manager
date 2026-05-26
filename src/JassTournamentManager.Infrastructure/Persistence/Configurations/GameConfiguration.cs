using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JassTournamentManager.Infrastructure.Persistence.Configurations
{
    public sealed class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("games");

            builder.ConfigureBaseEntity();

            builder.Property(game => game.PairingId)
                .HasColumnName("pairing_id")
                .IsRequired();

            builder.Property(game => game.GameNumber)
                .HasColumnName("game_number")
                .IsRequired();

            builder.Property(game => game.MatchBonusEnabled)
                .HasColumnName("match_bonus_enabled")
                .IsRequired();

            builder.Property(game => game.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(30)
                .IsRequired();

            builder.OwnsOne(game => game.Score, scoreBuilder =>
            {
                scoreBuilder.Property(gameScore => gameScore.TeamAPoints)
                    .HasColumnName("team_a_points");

                scoreBuilder.Property(gameScore => gameScore.TeamBPoints)
                    .HasColumnName("team_b_points");

                scoreBuilder.Property(gameScore => gameScore.TeamAMatchBonusAchieved)
                    .HasColumnName("team_a_match_bonus_achieved");

                scoreBuilder.Property(gameScore => gameScore.TeamBMatchBonusAchieved)
                    .HasColumnName("team_b_match_bonus_achieved");

                scoreBuilder.Property(gameScore => gameScore.EnteredByUserId)
                    .HasColumnName("entered_by_user_id");

                scoreBuilder.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(gameScore => gameScore.EnteredByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                scoreBuilder.Property(gameScore => gameScore.EnteredAt)
                    .HasColumnName("entered_at");
            });

            builder.HasIndex(game => game.PairingId)
                .HasDatabaseName("idx_game_pairing");

            builder.HasIndex(game => new
            {
                game.PairingId,
                game.GameNumber
            }).IsUnique()
            .HasDatabaseName("ux_games_pairing_game_number");
        }
    }
}
