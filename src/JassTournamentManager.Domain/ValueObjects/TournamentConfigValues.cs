using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.ValueObjects
{
    public sealed record TournamentConfigValues
    {
        public const int DefaultNumberOfRounds = 5;
        public const int DefaultGamesPerRound = 8;
        public const bool DefaultMatchBonusEnabled = true;
        public const bool DefaultIsFixedTeams = false;
        public const ScoreVisibility DefaultScoreVisibility = ScoreVisibility.HiddenDuringActiveTournament;

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
    }
}
