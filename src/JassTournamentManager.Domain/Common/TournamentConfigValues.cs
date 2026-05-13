using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.Common
{
    public sealed record TournamentConfigValues
    {
        private const int DefaultNumberOfRounds = 5;
        private const int DefaultGamesPerRound = 8;
        private const bool DefaultMatchBonusEnabled = true;
        private const bool DefaultIsFixedTeams = false;
        private const ScoreVisibility DefaultScoreVisibility = ScoreVisibility.HiddenDuringActiveTournament;

        public int NumberOfRounds { get; init; }

        public int GamesPerRound { get; init; }

        public bool MatchBonusEnabled { get; init; }

        public bool IsFixedTeams { get; init; }

        public ScoreVisibility ScoreVisibility { get; init; }

        public TournamentConfigValues(
            int numberOfRounds = DefaultNumberOfRounds,
            int gamesPerRound = DefaultGamesPerRound,
            bool matchBonusEnabled = DefaultMatchBonusEnabled,
            bool isFixedTeams = DefaultIsFixedTeams,
            ScoreVisibility scoreVisibility = DefaultScoreVisibility)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(numberOfRounds, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(gamesPerRound, 1);

            NumberOfRounds = numberOfRounds;
            GamesPerRound = gamesPerRound;
            MatchBonusEnabled = matchBonusEnabled;
            IsFixedTeams = isFixedTeams;
            ScoreVisibility = scoreVisibility;
        }

        public static void ValidateConfigValues(TournamentConfigValues configValues)
        {
            ArgumentNullException.ThrowIfNull(configValues);
            ArgumentOutOfRangeException.ThrowIfLessThan(configValues.NumberOfRounds, 1);
            ArgumentOutOfRangeException.ThrowIfLessThan(configValues.GamesPerRound, 1);
        }
    }
}
