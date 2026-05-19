using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class RoundTestData
    {
        public static Guid CreateRoundId() => Guid.NewGuid();

        public static int CreateRoundNumber() => 1;

        public static Round CreateRound() =>
            new(
                TournamentTestData.CreateTournamentId(),
                CreateRoundNumber());

        public static Round CreateRound(Guid tournamentId, int roundNumber = 1) =>
            new(
                tournamentId,
                roundNumber);
    }
}
