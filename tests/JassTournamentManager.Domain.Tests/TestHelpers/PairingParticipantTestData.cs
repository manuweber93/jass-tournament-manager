using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class PairingParticipantTestData
    {
        public static Guid CreateTournamentParticipantId() =>
            TournamentParticipantTestData.CreateTournamentParticipantId();

        public static Team CreateTeam() => Team.TeamA;

        public static PairingParticipant CreatePairingParticipant() => new(
            PairingTestData.CreatePairingId(),
            TournamentParticipantTestData.CreateTournamentParticipantId(),
            CreateTeam(),
            UserTestData.CreateUserId());
    }
}
