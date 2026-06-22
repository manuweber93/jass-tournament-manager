using JassTournamentManager.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.TournamentConfigs
{
    public sealed record TournamentConfigDto(
        [Range(1, int.MaxValue)]
        [DefaultValue(5)]
        int NumberOfRounds,

        [Range(1, int.MaxValue)]
        [DefaultValue(8)]
        int GamesPerRound,

        bool MatchBonusEnabled,

        bool IsFixedTeams,

        [Range(0, 2)]
        [DefaultValue(1)]
        ScoreVisibility ScoreVisibility);
}

