using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.Tests.TestHelpers;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class PairingTests
    {
        [Fact]
        public void Constructor_WithEmptyRoundId_ThrowsArgumentException()
        {
            var emptyRoundId = Guid.Empty;

            Action act = () => new Pairing(
                emptyRoundId,
                JassTableTestData.CreateJassTableId());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithEmptyJassTableId_ThrowsArgumentException()
        {
            var emptyJassTableId = Guid.Empty;

            Action act = () => new Pairing(
                RoundTestData.CreateRoundId(),
                emptyJassTableId);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesPairing()
        {
            var roundId = RoundTestData.CreateRoundId();
            var jassTableId = JassTableTestData.CreateJassTableId();

            var pairing = new Pairing(roundId, jassTableId, PairingStatus.Completed);

            pairing.RoundId.Should().Be(roundId);
            pairing.JassTableId.Should().Be(jassTableId);
            pairing.Status.Should().Be(PairingStatus.Completed);
        }

        [Fact]
        public void Constructor_DefaultStatus_IsPending()
        {
            var pairing = PairingTestData.CreatePairing();

            pairing.Status.Should().Be(PairingStatus.Pending);
        }

        [Fact]
        public void AddGame_WithTooLargeGameNumber_ThrowsInvalidOperationException()
        {
            var pairing = PairingTestData.CreatePairing();
            var game = GameTestData.CreateGame();

            Action act = () => pairing.AddGame(game, 0);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddGame_WithValidGame_AddsGame()
        {
            var pairing = PairingTestData.CreatePairing();
            var game = GameTestData.CreateGame();
            pairing.AddGame(game, 1);

            pairing.Games.Should().ContainSingle().
                Which.Should().Be(game);
        }

        [Fact]
        public void Complete_CompletesPairing()
        {
            var pairing = PairingTestData.CreatePairing();
            pairing.Complete();

            pairing.Status.Should().Be(PairingStatus.Completed);
        }

        [Fact]
        public void SetBackToPending_SetsStatusBackToPending()
        {
            var pairing = PairingTestData.CreatePairing();
            pairing.SetBackToPending();

            pairing.Status.Should().Be(PairingStatus.Pending);
        }
    }
}
