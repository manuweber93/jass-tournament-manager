using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.Tests.TestHelpers;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class TournamentTests
    {

        [Fact]
        public void Constructor_WithEmptyOrganizerId_ThrowsArgumentException()
        {
            var emptyGuid = Guid.Empty;

            Action act = () => new Tournament(emptyGuid,
                TournamentTestData.CreateTournamentDetails(),
                TournamentTestData.CreateTournamentConfigValues());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithoutTournamentDetails_ThrowsArgumentNullException()
        {
            Action act = () => new Tournament(Guid.NewGuid(),
                null!,
                TournamentTestData.CreateTournamentConfigValues());

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithoutTournamentConfigValues_ThrowsArgumentNullException()
        {
            Action act = () => new Tournament(Guid.NewGuid(),
                TournamentTestData.CreateTournamentDetails(),
                null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesTournament()
        {
            var organizerId = UserTestData.CreateUserId();
            var details = TournamentTestData.CreateTournamentDetails();
            var configValues = TournamentTestData.CreateTournamentConfigValues();
            var status = TournamentStatus.Completed;

            var tournament = new Tournament(
                organizerId,
                details,
                configValues,
                status);

            tournament.OrganizerId.Should().Be(organizerId);
            tournament.Details.Should().Be(details);
            tournament.ConfigValues.Should().Be(configValues);
            tournament.Status.Should().Be(status);
        }

        [Fact]
        public void Constructor_DefaultStatus_IsActive()
        {
            var tournament = TournamentTestData.CreateTournament();

            tournament.Status.Should().Be(TournamentStatus.Active);
        }

        [Fact]
        public void AddRound_WhenSameRoundAddedTwice_ThrowsInvalidOperationException()
        {
            var tournament = TournamentTestData.CreateTournament();
            var round = RoundTestData.CreateRound();
            tournament.AddRound(round);

            Action act = () => tournament.AddRound(round);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddRound_AddsRoundToTournament()
        {
            var tournament = TournamentTestData.CreateTournament();
            var round = RoundTestData.CreateRound();
            tournament.AddRound(round);

            tournament.Rounds.Should().ContainSingle()
                .Which.Should().Be(round);
        }

        [Fact]
        public void AddRound_UpdatesNumberOfRoundsInConfig()
        {
            var tournament = TournamentTestData.CreateTournament();
            var round = RoundTestData.CreateRound();
            tournament.AddRound(round);

            tournament.ConfigValues.NumberOfRounds.Should().Be(1);
        }

        [Fact]
        public void RemoveRound_WithNonExistingRoundId_ThrowsArgumentException()
        {
            var tournament = TournamentTestData.CreateTournament();
            var nonExistingRoundId = Guid.NewGuid();

            Action act = () => tournament.RemoveRound(nonExistingRoundId);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveRound_RemovesRoundFromTournament()
        {
            var tournament = TournamentTestData.CreateTournament();
            var round = RoundTestData.CreateRound();
            tournament.AddRound(round);
            tournament.RemoveRound(round.Id);

            tournament.Rounds.Should().BeEmpty();
        }

        [Fact]
        public void RemoveRound_UpdatesNumberOfRoundsInConfig()
        {
            var tournament = TournamentTestData.CreateTournament();
            var round = RoundTestData.CreateRound();
            tournament.AddRound(round);
            tournament.RemoveRound(round.Id);

            tournament.ConfigValues.NumberOfRounds.Should().Be(0);
        }

        [Fact]
        public void AddParticipant_WhenSameParticipantAddedTwice_ThrowsInvalidOperationException()
        {
            var tournament = TournamentTestData.CreateTournament();
            var participant = TournamentParticipantTestData.CreateTournamentParticipant();
            tournament.AddParticipant(participant);

            Action act = () => tournament.AddParticipant(participant);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void AddParticipant_AddsParticipantToTournament()
        {
            var tournament = TournamentTestData.CreateTournament();
            var participant = TournamentParticipantTestData.CreateTournamentParticipant();
            tournament.AddParticipant(participant);

            tournament.Participants.Should().ContainSingle()
                .Which.Should().Be(participant);
        }

        [Fact]
        public void RemoveParticipant_WithNonExistingParticipantId_ThrowsArgumentException()
        {
            var tournament = TournamentTestData.CreateTournament();
            var nonExistingParticipantId = Guid.NewGuid();

            Action act = () => tournament.RemoveParticipant(nonExistingParticipantId);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveParticipant_RemovesParticipantFromTournament()
        {
            var tournament = TournamentTestData.CreateTournament();
            var participant = TournamentParticipantTestData.CreateTournamentParticipant();
            tournament.AddParticipant(participant);
            tournament.RemoveParticipant(participant.Id);

            tournament.Participants.Should().BeEmpty();
        }

        [Fact]
        public void UpdateDetails_WithNullTournamentDetails_ThrowsArgumentException()
        {
            var tournament = TournamentTestData.CreateTournament();

            Action act = () => tournament.UpdateDetails(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UpdateDetails_WithValidTournamentDetails_UpdatesTournamentDetails()
        {
            var tournament = TournamentTestData.CreateTournament();
            var newTournamentDetails = TournamentTestData.CreateTournamentDetails();
            tournament.UpdateDetails(newTournamentDetails);

            tournament.Details.Should().Be(newTournamentDetails);
        }

        [Fact]
        public void UpdateConfigValues_WithNullConfigValues_ThrowsArgumentException()
        {
            var tournament = TournamentTestData.CreateTournament();

            Action act = () => tournament.UpdateConfigValues(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UpdateConfigValues_WithValidConfigValues_UpdatesConfigValues()
        {
            var tournament = TournamentTestData.CreateTournament();
            var newConfigValues = TournamentTestData.CreateTournamentConfigValues();
            tournament.UpdateConfigValues(newConfigValues);

            tournament.ConfigValues.Should().Be(newConfigValues);
        }

        [Fact]
        public void Complete_SetsStatusToCompleted()
        {
            var tournament = TournamentTestData.CreateTournament();
            tournament.Complete();

            tournament.Status.Should().Be(TournamentStatus.Completed);
        }

        [Fact]
        public void Cancel_SetsStatusToCancelled()
        {
            var tournament = TournamentTestData.CreateTournament();
            tournament.Cancel();

            tournament.Status.Should().Be(TournamentStatus.Cancelled);
        }

        [Fact]
        public void Reactivate_SetsStatusToActive()
        {
            var tournament = TournamentTestData.CreateTournament();
            tournament.Cancel();
            tournament.Reactivate();

            tournament.Status.Should().Be(TournamentStatus.Active);
        }
    }
}
