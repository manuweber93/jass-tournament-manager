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
        public void Constructor_WithEmptyEnteredByUserId_ThrowsArgumentException()
        {
            Action act = () => new GameScore(
                100,
                57,
                false,
                false,
                Guid.Empty);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithValidScore_CreatesGameScore()
        {
            var pointsTeamA = 100;
            var pointsTeamB = 57;
            var matchBonusAchieved = false;
            var enteredByUserId = UserTestData.CreateUserId();

            var gameScore = new GameScore(
                pointsTeamA,
                pointsTeamB,
                matchBonusAchieved,
                matchBonusAchieved,
                enteredByUserId);

            gameScore.TeamAPoints.Should().Be(pointsTeamA);
            gameScore.TeamBPoints.Should().Be(pointsTeamB);
            gameScore.TeamAMatchBonusAchieved.Should().Be(matchBonusAchieved);
            gameScore.TeamBMatchBonusAchieved.Should().Be(matchBonusAchieved);
            gameScore.EnteredByUserId.Should().Be(enteredByUserId);
            gameScore.EnteredAt.Should().NotBe(default);
        }

        [Fact]
        public void Constructor_WithTeamAMatchBonusAchieved_AddsMatchBonusPoints()
        {
            var gameScore = new GameScore(
                GameScore.TotalPointsPerGame,
                0,
                true,
                false,
                Guid.NewGuid());

            gameScore.TeamAPoints.Should().Be(GameScore.TotalPointsPerGame + GameScore.MatchBonusPoints);
        }

        [Fact]
        public void Constructor_WithTeamBMatchBonusAchieved_AddsMatchBonusPoints()
        {
            var gameScore = new GameScore(
                0,
                GameScore.TotalPointsPerGame,
                false,
                true,
                Guid.NewGuid());

            gameScore.TeamBPoints.Should().Be(GameScore.TotalPointsPerGame + GameScore.MatchBonusPoints);
        }

    }
}
