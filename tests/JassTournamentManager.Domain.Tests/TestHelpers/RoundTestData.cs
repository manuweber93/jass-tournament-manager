using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class RoundTestData
    {
        public static int CreateRoundNumber() => 1;

        public static Round CreateRound() =>
            new(
                TournamentTestData.CreateOrganizerId(),
                CreateRoundNumber());

    }
}
