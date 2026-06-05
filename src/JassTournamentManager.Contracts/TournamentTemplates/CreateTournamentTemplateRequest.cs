using JassTournamentManager.Contracts.TournamentConfigs;
using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.TournamentTemplates
{
    public sealed record CreateTournamentTemplateRequest(
        [property: Required]
        Guid OrganizerId,

        [property: Required]
        TournamentConfigDto Config,

        string? Location);
}
