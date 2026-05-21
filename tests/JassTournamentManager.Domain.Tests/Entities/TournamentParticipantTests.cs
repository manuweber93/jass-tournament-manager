using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.Tests.TestHelpers;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class TournamentParticipantTests
    {
        [Fact]
        public void Constructor_WithEmptyTournamentId_ThrowsArgumentException()
        {
            var emptyTournamentId = Guid.Empty;

            Action act = () => new TournamentParticipant(
                emptyTournamentId,
                UserTestData.CreateUserId(),
                TournamentParticipantTestData.CreateRegistrationMethod());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithEmptyUserId_ThrowsArgumentException()
        {
            var emptyUserId = Guid.Empty;

            Action act = () => new TournamentParticipant(
                TournamentTestData.CreateTournamentId(),
                emptyUserId,
                TournamentParticipantTestData.CreateRegistrationMethod());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesTournamentParticipant()
        {
            var tournamentId = TournamentTestData.CreateTournamentId();
            var userId = UserTestData.CreateUserId();
            var registrationMethod = TournamentParticipantTestData.CreateRegistrationMethod();
            var role = ParticipantRole.Organizer;
            var isPlaying = false;

            var participant = new TournamentParticipant(
                tournamentId,
                userId,
                registrationMethod,
                role,
                isPlaying);

            participant.TournamentId.Should().Be(tournamentId);
            participant.UserId.Should().Be(userId);
            participant.RegistrationMethod.Should().Be(registrationMethod);
            participant.Role.Should().Be(role);
            participant.IsPlaying.Should().Be(isPlaying);
        }

        [Fact]
        public void Constructor_DefaultRole_IsPlayer()
        {
            var participant = TournamentParticipantTestData.CreateTournamentParticipant();

            participant.Role.Should().Be(ParticipantRole.Player);
        }

        [Fact]
        public void Constructor_DefaultIsPlaying_IsTrue()
        {
            var participant = TournamentParticipantTestData.CreateTournamentParticipant();

            participant.IsPlaying.Should().BeTrue();
        }

        [Fact]
        public void UpdateDetails_WithValidValues_UpdatesDetails()
        {
            var participant = TournamentParticipantTestData.CreateTournamentParticipant();
            var newParticipantRole = ParticipantRole.Organizer;
            var newIsPlaying = false;

            participant.UpdateDetails(newParticipantRole, newIsPlaying);

            participant.Role.Should().Be(newParticipantRole);
            participant.IsPlaying.Should().Be(newIsPlaying);
        }
    }
}
