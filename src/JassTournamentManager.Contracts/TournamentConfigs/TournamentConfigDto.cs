using JassTournamentManager.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.TournamentConfigs
{
    public sealed record TournamentConfigDto(
        [property: Range(1, int.MaxValue)]
        [property: DefaultValue(5)]
        int NumberOfRounds,

        [property: Range(1, int.MaxValue)]
        [property: DefaultValue(8)]
        int GamesPerRound,

        bool MatchBonusEnabled,

        bool IsFixedTeams,

        [property: Range(0, 2)]
        [property: DefaultValue(1)]
        ScoreVisibility ScoreVisibility);
}
