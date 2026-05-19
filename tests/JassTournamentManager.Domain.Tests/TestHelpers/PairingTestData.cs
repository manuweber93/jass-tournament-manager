using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class PairingTestData
    {
        public static Guid CreatePairingId() => Guid.NewGuid();

        public static int CreateGamesPerRound() => 8;

        public static Pairing CreatePairing() => new(
            RoundTestData.CreateRoundId(),
            JassTableTestData.CreateJassTableId(),
            CreateGamesPerRound());

        public static Pairing CreatePairing(Guid roundId) => new(
            roundId,
            JassTableTestData.CreateJassTableId(),
            CreateGamesPerRound());

        public static void FillPairingWithParticipants(Pairing pairing)
        {
            pairing.AddParticipant(PairingParticipantTestData.CreatePairingParticipant(pairing.Id, TournamentParticipantTestData.CreateTournamentParticipantId(), Team.TeamA));
            pairing.AddParticipant(PairingParticipantTestData.CreatePairingParticipant(pairing.Id, TournamentParticipantTestData.CreateTournamentParticipantId(), Team.TeamA));
            pairing.AddParticipant(PairingParticipantTestData.CreatePairingParticipant(pairing.Id, TournamentParticipantTestData.CreateTournamentParticipantId(), Team.TeamB));
            pairing.AddParticipant(PairingParticipantTestData.CreatePairingParticipant(pairing.Id, TournamentParticipantTestData.CreateTournamentParticipantId(), Team.TeamB));
        }
    }
}
