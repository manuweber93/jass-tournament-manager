using JassTournamentManager.Contracts.TournamentConfigs;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Application.TournamentConfigs
{
    public static class TournamentConfigDtoMapper
    {
        public static TournamentConfigValues FromDto(TournamentConfigDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new TournamentConfigValues
            (
                dto.NumberOfRounds,
                dto.GamesPerRound,
                dto.MatchBonusEnabled,
                dto.IsFixedTeams,
                dto.ScoreVisibility
            );
        }

        public static TournamentConfigDto ToDto(TournamentConfigValues values)
        {
            ArgumentNullException.ThrowIfNull(values);

            return new TournamentConfigDto
            (
                values.NumberOfRounds,
                values.GamesPerRound,
                values.MatchBonusEnabled,
                values.IsFixedTeams,
                values.ScoreVisibility
            );
        }
    }
}
