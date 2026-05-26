using JassTournamentManager.Contracts.TournamentConfigs;

namespace JassTournamentManager.Contracts.TournamentTemplates
{
    public sealed record CreateTournamentTemplateRequest(
        Guid OrganizerId,
        TournamentConfigDto Config,
        string? Location);
}
