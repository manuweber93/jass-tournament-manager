using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.Tests.TestHelpers;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class PairingParticipantTests
    {
        [Fact]
        public void Constructor_WithEmptyPairingId_ThrowsArgumentsException()
        {
            var emptyPairingId = Guid.Empty;

            Action act = () => new PairingParticipant(
                emptyPairingId,
                PairingParticipantTestData.CreateTournamentParticipantId(),
                PairingParticipantTestData.CreateTeam(),
                UserTestData.CreateUserId());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithEmptyTournamentParticipantId_ThrowsArgumentsException()
        {
            var emptyTournamentParticipantId = Guid.Empty;

            Action act = () => new PairingParticipant(
                PairingTestData.CreatePairingId(),
                emptyTournamentParticipantId,
                PairingParticipantTestData.CreateTeam(),
                UserTestData.CreateUserId());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithEmptyEnteredByUserId_ThrowsArgumentException()
        {
            var emptyEnteredByUserId = Guid.Empty;

            Action act = () => new PairingParticipant(
                PairingTestData.CreatePairingId(),
                PairingParticipantTestData.CreateTournamentParticipantId(),
                PairingParticipantTestData.CreateTeam(),
                emptyEnteredByUserId);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesPairingParticipant()
        {
            var pairingId = PairingTestData.CreatePairingId();
            var tournamentParticipantId = PairingParticipantTestData.CreateTournamentParticipantId();
            var team = PairingParticipantTestData.CreateTeam();
            var enteredByUserId = UserTestData.CreateUserId();

            var pairingParticipant = new PairingParticipant(
                pairingId,
                tournamentParticipantId,
                team,
                enteredByUserId);

            pairingParticipant.PairingId.Should().Be(pairingId);
            pairingParticipant.TournamentParticipantId.Should().Be(tournamentParticipantId);
            pairingParticipant.Team.Should().Be(team);
            pairingParticipant.EnteredByUserId.Should().Be(enteredByUserId);
        }

        [Fact]
        public void UpdateTournamentParticipant_WithEmptyTournamentParticipantId_ThrowsArgumentException()
        {
            var emptyTournamentParticipantId = Guid.Empty;
            var pairingParticipant = PairingParticipantTestData.CreatePairingParticipant();

            Action act = () => pairingParticipant.UpdateTournamentParticipant(emptyTournamentParticipantId);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void UpdateTournamentParticipant_WithValidTournamentParticipantId_UpdatesTournamentParticipant()
        {
            var pairingParticipant = PairingParticipantTestData.CreatePairingParticipant();
            var newTournamentParticipantId = PairingParticipantTestData.CreateTournamentParticipantId();

            pairingParticipant.UpdateTournamentParticipant(newTournamentParticipantId);

            pairingParticipant.TournamentParticipantId.Should().Be(newTournamentParticipantId);
        }

        [Fact]
        public void UpdateTeam_WithValidTeam_UpdatesTeam()
        {
            var pairingParticipant = PairingParticipantTestData.CreatePairingParticipant();
            var newTeam = Team.TeamB;

            pairingParticipant.UpdateTeam(newTeam);

            pairingParticipant.Team.Should().Be(newTeam);
        }
    }
}
