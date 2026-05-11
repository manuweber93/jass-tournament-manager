using JassTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JassTournamentManager.Domain.Common
{
    public sealed class TournamentConfigValues
    {
        private const int DefaultNumberOfRounds = 5;
        private const int DefaultGamesPerRound = 8;
        private const bool DefaultMatchBonusEnabled = true;
        private const bool DefaultFixedTeams = false;
        private const ScoreVisibility DefaultScoreVisibility = ScoreVisibility.HiddenDuringActiveTournament;

        public int NumberOfRounds { get; }

        public int GamesPerRound { get; }

        public bool MatchBonusEnabled { get; }

        public bool FixedTeams { get; }

        public ScoreVisibility ScoreVisibility { get; }

        public TournamentConfigValues(
            int numberOfRounds = DefaultNumberOfRounds,
            int gamesPerRound = DefaultGamesPerRound,
            bool matchBonusEnabled = DefaultMatchBonusEnabled,
            bool fixedTeams = DefaultFixedTeams,
            ScoreVisibility scoreVisibility = DefaultScoreVisibility)
        {
            if (numberOfRounds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfRounds), "Number of rounds must be a positive integer.");
            }

            if (gamesPerRound <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gamesPerRound), "Games per round must be a positive integer.");
            }

            NumberOfRounds = numberOfRounds;
            GamesPerRound = gamesPerRound;
            MatchBonusEnabled = matchBonusEnabled;
            FixedTeams = fixedTeams;
            ScoreVisibility = scoreVisibility;
        }
    }
}
