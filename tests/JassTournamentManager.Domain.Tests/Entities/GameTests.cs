using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.Tests.TestHelpers;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class GameTests
    {
        [Fact]
        public void Constructor_WithEmptyPairingId_ShouldThrowArgumentException()
        {
            var emptyGuid = Guid.Empty;

            Action act = () => new Game(emptyGuid, 1);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithInvalidGameNumber_ShouldThrowArgumentOutOfRangeException()
        {
            var pairingId = PairingTestData.CreatePairingId();
            var invalidGameNumber = 0;

            Action act = () => new Game(pairingId, invalidGameNumber);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Constructor_DefaultStatus_ShouldBePending()
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
        public void Complete_ShouldSetStatusToCompleted()
        {
            var game = GameTestData.CreateGame();
            game.Complete();

            game.Status.Should().Be(GameStatus.Completed);
        }

        [Fact]
        public void SetBackToPending_ShouldSetStatusToPending()
        {
            var game = GameTestData.CreateGame();
            game.Complete();
            game.SetBackToPending();

            game.Status.Should().Be(GameStatus.Pending);
        }
    }
}
