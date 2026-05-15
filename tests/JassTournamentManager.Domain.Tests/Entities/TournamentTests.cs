using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.Tests.TestHelpers;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class TournamentTests
    {

        [Fact]
        public void Constructor_WithEmptyOrganizerId_ShouldThrowArgumentException()
        {
            var emptyGuid = Guid.Empty;

            Action act = () => new Tournament(emptyGuid,
                TournamentTestData.CreateTournamentDetails(),
                TournamentTestData.CreateTournamentConfigValues());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_DefaultStatus_ShouldBeActive()
        {
            var tournament = TournamentTestData.CreateTournament();

            tournament.Status.Should().Be(TournamentStatus.Active);
        }

        [Fact]
        public void AddRound_WhenSameRoundAddedTwice_ShouldThrowInvalidOperationException()
        {
            var tournament = TournamentTestData.CreateTournament();
            var round = RoundTestData.CreateRound();
            tournament.AddRound(round);

            Action act = () => tournament.AddRound(round);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddRound_ShouldAddRoundToTournament()
        {
            var tournament = TournamentTestData.CreateTournament();
            var round = RoundTestData.CreateRound();
            tournament.AddRound(round);

            tournament.Rounds.Should().ContainSingle()
                .Which.Should().Be(round);
        }

        [Fact]
        public void AddRound_ShouldUpdateNumberOfRoundsInConfig()
        {
            var tournament = TournamentTestData.CreateTournament();
            var round = RoundTestData.CreateRound();
            tournament.AddRound(round);

            tournament.ConfigValues.NumberOfRounds.Should().Be(1);
        }

        [Fact]
        public void RemoveRound_WithNonExistingRoundId_ShouldThrowArgumentException()
        {
            var tournament = TournamentTestData.CreateTournament();
            var nonExistingRoundId = Guid.NewGuid();

            Action act = () => tournament.RemoveRound(nonExistingRoundId);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveRound_ShouldRemoveRoundFromTournament()
        {
            var tournament = TournamentTestData.CreateTournament();
            var round = RoundTestData.CreateRound();
            tournament.AddRound(round);
            tournament.RemoveRound(round.Id);

            tournament.Rounds.Should().BeEmpty();
        }

        [Fact]
        public void RemoveRound_ShouldUpdateNumberOfRoundsInConfig()
        {
            var tournament = TournamentTestData.CreateTournament();
            var round = RoundTestData.CreateRound();
            tournament.AddRound(round);
            tournament.RemoveRound(round.Id);

            tournament.ConfigValues.NumberOfRounds.Should().Be(0);
        }

        [Fact]
        public void AddParticipant_WhenSameParticipantAddedTwice_ShouldThrowInvalidOperationException()
        {
            var tournament = TournamentTestData.CreateTournament();
            var participant = TournamentParticipantTestData.CreateTournamentParticipant();
            tournament.AddParticipant(participant);

            Action act = () => tournament.AddParticipant(participant);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddParticipant_ShouldAddParticipantToTournament()
        {
            var tournament = TournamentTestData.CreateTournament();
            var participant = TournamentParticipantTestData.CreateTournamentParticipant();
            tournament.AddParticipant(participant);

            tournament.Participants.Should().ContainSingle()
                .Which.Should().Be(participant);
        }

        [Fact]
        public void RemoveParticipant_WithNonExistingParticipantId_ShouldThrowArgumentException()
        {
            var tournament = TournamentTestData.CreateTournament();
            var nonExistingParticipantId = Guid.NewGuid();

            Action act = () => tournament.RemoveParticipant(nonExistingParticipantId);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveParticipant_ShouldRemoveParticipantFromTournament()
        {
            var tournament = TournamentTestData.CreateTournament();
            var participant = TournamentParticipantTestData.CreateTournamentParticipant();
            tournament.AddParticipant(participant);
            tournament.RemoveParticipant(participant.Id);

            tournament.Participants.Should().BeEmpty();
        }

        [Fact]
        public void UpdateDetails_WithNullTournamentDetails_ShouldThrowArgumentException()
        {
            var tournament = TournamentTestData.CreateTournament();

            Action act = () => tournament.UpdateDetails(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UpdateConfigValues_WithNullConfigValues_ShouldThrowArgumentException()
        {
            var tournament = TournamentTestData.CreateTournament();

            Action act = () => tournament.UpdateConfigValues(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Complete_ShouldSetStatusToCompleted()
        {
            var tournament = TournamentTestData.CreateTournament();
            tournament.Complete();

            tournament.Status.Should().Be(TournamentStatus.Completed);
        }

        [Fact]
        public void Cancel_ShouldSetStatusToCancelled()
        {
            var tournament = TournamentTestData.CreateTournament();
            tournament.Cancel();

            tournament.Status.Should().Be(TournamentStatus.Cancelled);
        }

        [Fact]
        public void Reactivate_ShouldSetStatusToActive()
        {
            var tournament = TournamentTestData.CreateTournament();
            tournament.Cancel();
            tournament.Reactivate();

            tournament.Status.Should().Be(TournamentStatus.Active);
        }

    }
}
