using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class TournamentTemplateTestData
    {
        public static TournamentTemplate CreateTournamentTemplate() => new(
            UserTestData.CreateUserId(),
            TournamentTestData.CreateTournamentConfigValues(),
            TournamentTestData.CreateLocation());
    }
}
