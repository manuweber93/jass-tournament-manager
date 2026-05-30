using JassTournamentManager.Contracts.TournamentConfigs;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Application.Tests.TournamentConfigs
{
    internal static class TournamentTestData
    {
        public static TournamentConfigValues CreateTournamentConfigValues() => new();

        public static TournamentConfigDto CreateTournamentConfigDto(int numberOfRounds = 5) => new(
            numberOfRounds,
            8,
            true,
            false,
            ScoreVisibility.HiddenDuringActiveTournament);
    }
}
