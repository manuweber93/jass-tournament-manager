using JassTournamentManager.Contracts.TournamentConfigs;
using JassTournamentManager.Contracts.TournamentTemplates;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Api.Tests.TournamentTemplates
{
    internal static class TournamentTemplatesControllerTestData
    {
        public static Guid CreateTournamentTemplateId() => Guid.NewGuid();

        public static Guid CreateOrganizerId() => Guid.NewGuid();

        public static TournamentConfigDto CreateConfig() => new(
            5,
            8,
            true,
            false,
            ScoreVisibility.HiddenDuringActiveTournament);

        public static string CreateLocation() => "Zeigerhüsli";

        public static CreateTournamentTemplateRequest CreateCreateTournamentTemplateRequest() => new(
            CreateConfig(),
            CreateLocation());

        public static TournamentTemplateResponse CreateTournamentTemplateResponse(Guid? id = null) => new(
            id ?? CreateTournamentTemplateId(),
            CreateOrganizerId(),
            CreateConfig(),
            CreateLocation(),
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);
    }
}
