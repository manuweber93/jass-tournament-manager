using FluentAssertions;
using JassTournamentManager.Domain.Tests.TestHelpers;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Tests.ValueObjects
{
    public class GameScoreTests
    {
        [Theory]
        [InlineData(-1, 100)]
        [InlineData(100, -1)]
        public void Constructor_WithNegativePoints_ThrowsArgumentOutOfRangeException(int pointsTeamA, int pointsTeamB)
        {
            Action act = () => new GameScore(
                pointsTeamA,
                pointsTeamB,
                false,
                false,
                Guid.NewGuid());

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(100, 56)]
        [InlineData(58, 100)]
        [InlineData(0, 100)]
        [InlineData(100, 0)]
        [InlineData(0, 0)]
        [InlineData(157, 157)]
        public void Constructor_WithPointsNotAddingUp_ThrowsInvalidOperationException(int pointsTeamA, int pointsTeamB)
        {
            Action act = () => new GameScore(
                pointsTeamA,
                pointsTeamB,
                false,
                false,
                Guid.NewGuid());

            act.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(100, 57, true, false)]
        [InlineData(100, 57, false, true)]
        [InlineData(57, 100, true, false)]
        [InlineData(57, 100, false, true)]
        public void Constructor_WithMatchBonusButNotAllPointsWon_ThrowsInvalidOperationException(
            int pointsTeamA, int pointsTeamB, bool teamAMatchBonusAchieved, bool teamBMatchBonusAchieved)
        {
            Action act = () => new GameScore(
                pointsTeamA,
                pointsTeamB,
                teamAMatchBonusAchieved,
                teamBMatchBonusAchieved,
                Guid.NewGuid());

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Constructor_WithBothTeamsAchievingMatchBonus_ThrowsInvalidOperationException()
        {
            Action act = () => new GameScore(
                157,
                0,
                true,
                true,
                Guid.NewGuid());

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Constructor_WithTeamAMatchBonusAchieved_AddMatchBonusPoints()
        {
            var gameScore = new GameScore(
                157,
                0,
                true,
                false,
                Guid.NewGuid());

            gameScore.TeamAPoints.Should().Be(GameScore.TotalPointsPerGame + GameScore.MatchBonusPoints);
        }

        [Fact]
        public void Constructor_WithTeamBMatchBonusAchieved_AddMatchBonusPoints()
        {
            var gameScore = new GameScore(
                0,
                157,
                false,
                true,
                Guid.NewGuid());

            gameScore.TeamBPoints.Should().Be(GameScore.TotalPointsPerGame + GameScore.MatchBonusPoints);
        }

    }
}
