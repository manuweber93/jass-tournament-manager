using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Contracts.TournamentConfigs
{
    public sealed record TournamentConfigDto(
        int NumberOfRounds,
        int GamesPerRound,
        bool MatchBonusEnabled,
        bool IsFixedTeams,
        ScoreVisibility ScoreVisibility);
}
