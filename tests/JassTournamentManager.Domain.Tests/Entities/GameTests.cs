using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.Tests.TestHelpers;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class GameTests
    {
        [Fact]
        public void Constructor_WithEmptyPairingId_ThrowsArgumentException()
        {
            var emptyGuid = Guid.Empty;

            Action act = () => new Game(emptyGuid, 1);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithInvalidGameNumber_ThrowsArgumentOutOfRangeException()
        {
            var pairingId = PairingTestData.CreatePairingId();
            var invalidGameNumber = 0;

            Action act = () => new Game(pairingId, invalidGameNumber);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesGame()
        {
            var pairingId = PairingTestData.CreatePairingId();
            var gameNumber = GameTestData.CreateGameNumber();
            var status = GameStatus.Completed;

            var game = new Game(pairingId, gameNumber, status);

            game.PairingId.Should().Be(pairingId);
            game.GameNumber.Should().Be(gameNumber);
            game.Status.Should().Be(status);
        }

        [Fact]
        public void Constructor_DefaultStatus_IsPending()
        {
            var game = GameTestData.CreateGame();

            game.Status.Should().Be(GameStatus.Pending);
        }

        [Fact]
        public void SetScore_WithNullScore_ThrowsArgumentNullException()
        {
            var game = GameTestData.CreateGame();

            Action act = () => game.SetScore(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void SetScore_WithValidScore_SetsScore()
        {
            var game = GameTestData.CreateGame();
            var score = GameTestData.CreateGameScore();
            
            game.SetScore(score);

            game.Score.Should().Be(score);
        }

        [Fact]
        public void SetScore_WithValidScore_CompletesGame()
        {
            var game = GameTestData.CreateGame();
            var score = GameTestData.CreateGameScore();

            game.SetScore(score);

            game.Status.Should().Be(GameStatus.Completed);
        }

        [Fact]
        public void SetScore_WithMatchBonusAndMatchBonusDisabled_ThrowsInvalidOperationException()
        {
            var game = GameTestData.CreateGame();
            var score = new GameScore(
                GameScore.TotalPointsPerGame,
                0,
                true,
                false,
                UserTestData.CreateUserId());

            Action act = () => game.SetScore(score, matchBonusEnabled: false);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Complete_WithoutScore_ThrowsInvalidOperationException()
        {
            var game = GameTestData.CreateGame();

            Action act = () => game.Complete();

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Complete_SetsStatusToCompleted()
        {
            var game = GameTestData.CreateGame();
            game.SetScore(GameTestData.CreateGameScore());
            game.Complete();

            game.Status.Should().Be(GameStatus.Completed);
        }

        [Fact]
        public void SetBackToPending_SetsStatusToPending()
        {
            var game = GameTestData.CreateGame();
            game.SetScore(GameTestData.CreateGameScore());
            game.SetBackToPending();

            game.Status.Should().Be(GameStatus.Pending);
        }

        [Fact]
        public void SetBackToPending_KeepsScore()
        {
            var game = GameTestData.CreateGame();
            var score = GameTestData.CreateGameScore();
            game.SetScore(score);

            game.SetBackToPending();

            game.Score.Should().Be(score);
        }
    }
}
