using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.Tests.TestHelpers;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class RoundTests
    {
        [Fact]
        public void Constructor_WithEmptyTournamentId_ThrowsArgumentException()
        {
            var emptyTournamentId = Guid.Empty;

            Action act = () => new Round(
                emptyTournamentId,
                RoundTestData.CreateRoundNumber());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithInvalidRoundNumber_ThrowsArgumentOutOfRangeException()
        {
            var invalidRoundNumber = 0;

            Action act = () => new Round(
                TournamentTestData.CreateTournamentId(),
                invalidRoundNumber);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesRound()
        {
            var tournamentId = TournamentTestData.CreateTournamentId();
            var roundNumber = RoundTestData.CreateRoundNumber();
            var status = RoundStatus.Completed;

            var round = new Round(tournamentId, roundNumber, status);

            round.TournamentId.Should().Be(tournamentId);
            round.RoundNumber.Should().Be(roundNumber);
            round.Status.Should().Be(status);
        }

        [Fact]
        public void Constructor_DefaultStatus_IsPending()
        {
            var round = RoundTestData.CreateRound();

            round.Status.Should().Be(RoundStatus.Pending);
        }

        [Fact]
        public void AddPairing_WithValidPairing_AddsPairing()
        {
            var round = RoundTestData.CreateRound();
            var pairing = PairingTestData.CreatePairing(round.Id);

            round.AddPairing(pairing);

            round.Pairings.Should().ContainSingle()
                .Which.Should().Be(pairing);
        }

        [Fact]
        public void AddPairing_WithDifferentRoundId_ThrowsInvalidOperationException()
        {
            var round = RoundTestData.CreateRound();
            var pairing = PairingTestData.CreatePairing();

            Action act = () => round.AddPairing(pairing);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddPairing_WithDuplicateJassTableId_ThrowsInvalidOperationException()
        {
            var round = RoundTestData.CreateRound();
            var jassTableId = JassTableTestData.CreateJassTableId();
            var pairing = new Pairing(round.Id, jassTableId, PairingTestData.CreateGamesPerRound());
            round.AddPairing(pairing);
            var pairingWithDuplicateJassTableId = new Pairing(round.Id, jassTableId, PairingTestData.CreateGamesPerRound());

            Action act = () => round.AddPairing(pairingWithDuplicateJassTableId);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Activate_SetsStatusToActive()
        {
            var round = RoundTestData.CreateRound();

            round.Activate();

            round.Status.Should().Be(RoundStatus.Active);
        }

        [Fact]
        public void Complete_SetsStatusToCompleted()
        {
            var round = RoundTestData.CreateRound();

            round.Complete();

            round.Status.Should().Be(RoundStatus.Completed);
        }

        [Fact]
        public void SetBackToPending_SetsStatusToPending()
        {
            var round = RoundTestData.CreateRound();
            round.Activate();

            round.SetBackToPending();

            round.Status.Should().Be(RoundStatus.Pending);
        }
    }
}
