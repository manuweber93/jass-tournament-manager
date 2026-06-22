using JassTournamentManager.Application.Tests.TournamentConfigs;
using JassTournamentManager.Contracts.TournamentConfigs;
using JassTournamentManager.Contracts.TournamentTemplates;
using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Tests.TournamentTemplates
{
    internal static class TournamentTemplateTestData
    {
        public static Guid CreateOrganizerId() => Guid.NewGuid();

        public static string CreateLocation() => "Zeigerhüsli";

        public static string CreateTooLongLocation() => new('a', 201);

        public static CreateTournamentTemplateRequest CreateCreateTournamentTemplateRequest(
            TournamentConfigDto? config = null,
            string? location = null) => new(
                config ?? TournamentTestData.CreateTournamentConfigDto(),
                location ?? CreateLocation());

        public static CreateTournamentTemplateRequest CreateCreateTournamentTemplateRequestWithNullConfig(Guid? userId = null) => new(
            null!,
            CreateLocation());

        public static TournamentTemplate CreateTournamentTemplate(Guid? organizerId = null) => new(
            organizerId ?? CreateOrganizerId(),
            TournamentTestData.CreateTournamentConfigValues(),
            CreateLocation());
    }
}
