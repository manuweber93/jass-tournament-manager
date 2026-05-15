using FluentAssertions;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Tests.ValueObjects
{
    public class TournamentConfigValuesTests
    {
        [Fact]
        public void Constructor_WithInvalidNumberOfRounds_ShouldThrowArgumentOutOfRangeException()
        {
            var invalidNumberOfRounds = 0;

            Action act = () => new TournamentConfigValues(
                invalidNumberOfRounds);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Constructor_WithInvalidGamesPerRound_ShouldThrowArgumentOutOfRangeException()
        {
            var invalidGamesPerRound = 0;

            Action act = () => new TournamentConfigValues(
                default,
                invalidGamesPerRound);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
