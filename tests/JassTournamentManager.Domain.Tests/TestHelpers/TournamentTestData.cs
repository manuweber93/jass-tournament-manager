using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Services;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class TournamentTestData
    {
        public static Guid CreateOrganizerId() => Guid.NewGuid();

        public static string CreateTournamentName() => "Test Tournament";

        public static DateOnly CreateTournamentDate() => new(2026, 5, 13);

        public static string CreateTournamentCode() => TournamentCodeGenerator.GenerateTournamentCode();

        public static TournamentConfigValues CreateTournamentConfigValues() => new();

        public static TournamentDetails CreateTournamentDetails() =>
            new(
                CreateTournamentName(),
                null,
                CreateTournamentDate(),
                CreateTournamentCode());

        public static Tournament CreateTournament() => 
            new(
                CreateOrganizerId(),
                CreateTournamentDetails(),
                CreateTournamentConfigValues());
    }
}
