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
                JassTableTestData.CreateJassTableId(),
                PairingTestData.CreateGamesPerRound());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithEmptyJassTableId_ThrowsArgumentException()
        {
            var emptyJassTableId = Guid.Empty;

            Action act = () => new Pairing(
                RoundTestData.CreateRoundId(),
                emptyJassTableId,
                PairingTestData.CreateGamesPerRound());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithInvalidGamesPerRound_ThrowsArgumentOutOfRangeException()
        {
            var invalidGamesPerRound = 0;

            Action act = () => new Pairing(
                RoundTestData.CreateRoundId(),
                JassTableTestData.CreateJassTableId(),
                invalidGamesPerRound);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesPairing()
        {
            var roundId = RoundTestData.CreateRoundId();
            var jassTableId = JassTableTestData.CreateJassTableId();
            var gamesPerRound = PairingTestData.CreateGamesPerRound();
            var status = PairingStatus.Completed;

            var pairing = new Pairing(roundId, jassTableId, gamesPerRound, status);

            pairing.RoundId.Should().Be(roundId);
            pairing.JassTableId.Should().Be(jassTableId);
            pairing.GamesPerRound.Should().Be(gamesPerRound);
            pairing.Status.Should().Be(status);
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
            var game = GameTestData.CreateGame(pairing.Id, pairing.GamesPerRound + 1);

            Action act = () => pairing.AddGame(game);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddGame_WithDifferentPairingId_ThrowsInvalidOperationException()
        {
            var pairing = PairingTestData.CreatePairing();
            var game = GameTestData.CreateGame();

            Action act = () => pairing.AddGame(game);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddGame_WithDuplicateGameNumber_ThrowsInvalidOperationException()
        {
            var pairing = PairingTestData.CreatePairing();
            var game = GameTestData.CreateGame(pairing.Id, 1);
            pairing.AddGame(game);
            var duplicateGame = GameTestData.CreateGame(pairing.Id, 1);

            Action act = () => pairing.AddGame(duplicateGame);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddGame_WithValidGame_AddsGame()
        {
            var pairing = PairingTestData.CreatePairing();
            var game = GameTestData.CreateGame(pairing.Id);
            pairing.AddGame(game);

            pairing.Games.Should().ContainSingle().
                Which.Should().Be(game);
        }

        [Fact]
        public void AddParticipant_WithDifferentPairingId_ThrowsInvalidOperationException()
        {
            var pairing = PairingTestData.CreatePairing();
            var participant = PairingParticipantTestData.CreatePairingParticipant();

            Action act = () => pairing.AddParticipant(participant);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddParticipant_WithDuplicateTournamentParticipantId_ThrowsInvalidOperationException()
        {
            var pairing = PairingTestData.CreatePairing();
            var tournamentParticipantId = TournamentParticipantTestData.CreateTournamentParticipantId();
            var pairingParticipant = PairingParticipantTestData.CreatePairingParticipant(pairing.Id, tournamentParticipantId, Team.TeamA);
            pairing.AddParticipant(pairingParticipant);
            var duplicateParticipant = PairingParticipantTestData.CreatePairingParticipant(pairing.Id, tournamentParticipantId, Team.TeamB);

            Action act = () => pairing.AddParticipant(duplicateParticipant);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddParticipant_WhenPairingAlreadyHasFourParticipants_ThrowsInvalidOperationException()
        {
            var pairing = PairingTestData.CreatePairing();
            PairingTestData.FillPairingWithParticipants(pairing);
            var additionalParticipant = PairingParticipantTestData.CreatePairingParticipant(
                pairing.Id,
                TournamentParticipantTestData.CreateTournamentParticipantId(),
                Team.TeamA);

            Action act = () => pairing.AddParticipant(additionalParticipant);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddParticipant_WhenTeamAlreadyHasTwoParticipants_ThrowsInvalidOperationException()
        {
            var pairing = PairingTestData.CreatePairing();
            pairing.AddParticipant(PairingParticipantTestData.CreatePairingParticipant(pairing.Id, TournamentParticipantTestData.CreateTournamentParticipantId(), Team.TeamA));
            pairing.AddParticipant(PairingParticipantTestData.CreatePairingParticipant(pairing.Id, TournamentParticipantTestData.CreateTournamentParticipantId(), Team.TeamA));
            var thirdTeamAParticipant = PairingParticipantTestData.CreatePairingParticipant(
                pairing.Id,
                TournamentParticipantTestData.CreateTournamentParticipantId(),
                Team.TeamA);

            Action act = () => pairing.AddParticipant(thirdTeamAParticipant);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddParticipant_WithValidParticipant_AddsParticipant()
        {
            var pairing = PairingTestData.CreatePairing();
            var participant = PairingParticipantTestData.CreatePairingParticipant(
                pairing.Id,
                TournamentParticipantTestData.CreateTournamentParticipantId(),
                Team.TeamA);

            pairing.AddParticipant(participant);

            pairing.Participants.Should().ContainSingle()
                .Which.Should().Be(participant);
        }

        [Fact]
        public void Complete_CompletesPairing()
        {
            var pairing = PairingTestData.CreatePairing();
            PairingTestData.FillPairingWithParticipants(pairing);

            pairing.Complete();

            pairing.Status.Should().Be(PairingStatus.Completed);
        }

        [Fact]
        public void Complete_WithLessThanFourParticipants_ThrowsInvalidOperationException()
        {
            var pairing = PairingTestData.CreatePairing();

            Action act = () => pairing.Complete();

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Complete_WithPendingGame_ThrowsInvalidOperationException()
        {
            var pairing = PairingTestData.CreatePairing();
            PairingTestData.FillPairingWithParticipants(pairing);
            var game = GameTestData.CreateGame(pairing.Id);
            pairing.AddGame(game);

            Action act = () => pairing.Complete();

            act.Should().Throw<InvalidOperationException>();
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
