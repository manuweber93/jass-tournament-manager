using JassTournamentManager.Contracts.TournamentConfigs;

namespace JassTournamentManager.Contracts.TournamentTemplates
{
    public sealed record TournamentTemplateResponse(
        Guid Id,
        Guid OrganizerId,
        TournamentConfigDto Config,
        string? Location,
        DateTimeOffset CreatedAt,
        DateTimeOffset UpdatedAt);
}
