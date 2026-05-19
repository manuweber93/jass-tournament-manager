using FluentAssertions;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Tests.ValueObjects
{
    public class TournamentConfigValuesTests
    {
        [Fact]
        public void Constructor_WithInvalidNumberOfRounds_ThrowsArgumentOutOfRangeException()
        {
            var invalidNumberOfRounds = 0;

            Action act = () => new TournamentConfigValues(
                invalidNumberOfRounds);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Constructor_WithInvalidGamesPerRound_ThrowsArgumentOutOfRangeException()
        {
            var invalidGamesPerRound = 0;

            Action act = () => new TournamentConfigValues(
                default,
                invalidGamesPerRound);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Constructor_WithDefaultValues_CreatesDefaultConfigValues()
        {
            var configValues = new TournamentConfigValues();

            configValues.NumberOfRounds.Should().Be(TournamentConfigValues.DefaultNumberOfRounds);
            configValues.GamesPerRound.Should().Be(TournamentConfigValues.DefaultGamesPerRound);
            configValues.MatchBonusEnabled.Should().Be(TournamentConfigValues.DefaultMatchBonusEnabled);
            configValues.IsFixedTeams.Should().Be(TournamentConfigValues.DefaultIsFixedTeams);
            configValues.ScoreVisibility.Should().Be(TournamentConfigValues.DefaultScoreVisibility);
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesConfigValues()
        {
            var numberOfRounds = 7;
            var gamesPerRound = 10;
            var matchBonusEnabled = false;
            var isFixedTeams = true;
            var scoreVisibility = ScoreVisibility.OrganizerOnly;

            var configValues = new TournamentConfigValues(
                numberOfRounds,
                gamesPerRound,
                matchBonusEnabled,
                isFixedTeams,
                scoreVisibility);

            configValues.NumberOfRounds.Should().Be(numberOfRounds);
            configValues.GamesPerRound.Should().Be(gamesPerRound);
            configValues.MatchBonusEnabled.Should().Be(matchBonusEnabled);
            configValues.IsFixedTeams.Should().Be(isFixedTeams);
            configValues.ScoreVisibility.Should().Be(scoreVisibility);
        }
    }
}
