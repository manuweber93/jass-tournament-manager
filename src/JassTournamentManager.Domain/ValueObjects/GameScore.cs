using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.ValueObjects
{
    public sealed record GameScore
    {
        public const int TotalPointsPerGame = 157;
        public const int MatchBonusPoints = 100;

        public int TeamAPoints { get; }
        public int TeamBPoints { get; }

        public bool TeamAMatchBonusAchieved { get; }
        public bool TeamBMatchBonusAchieved { get; }

        public Guid EnteredBy { get; }

        public DateTimeOffset EnteredAt { get; }

        public GameScore(
            int teamAPoints,
            int teamBPoints,
            bool teamAMatchBonusAchieved,
            bool teamBMatchBonusAchieved,
            Guid enteredBy)
        {
            Validate(
                teamAPoints,
                teamBPoints,
                teamAMatchBonusAchieved,
                teamBMatchBonusAchieved,
                enteredBy);

            TeamAPoints = ApplyMatchBonus(teamAPoints, teamAMatchBonusAchieved);
            TeamBPoints = ApplyMatchBonus(teamBPoints, teamBMatchBonusAchieved);

            TeamAMatchBonusAchieved = teamAMatchBonusAchieved;
            TeamBMatchBonusAchieved = teamBMatchBonusAchieved;

            EnteredBy = enteredBy;
            EnteredAt = DateTimeOffset.UtcNow;
        }

        private static void Validate(
            int teamAPoints,
            int teamBPoints,
            bool teamAMatchBonusAchieved,
            bool teamBMatchBonusAchieved,
            Guid enteredBy)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(teamAPoints);
            ArgumentOutOfRangeException.ThrowIfNegative(teamBPoints);

            if (teamAPoints + teamBPoints != TotalPointsPerGame)
            {
                throw new InvalidOperationException(
                    $"Total points must equal {TotalPointsPerGame}.");
            }

            if (teamAMatchBonusAchieved && teamAPoints != TotalPointsPerGame)
            {
                throw new InvalidOperationException(
                    "Team A can only receive match bonus when winning all points.");
            }

            if (teamBMatchBonusAchieved && teamBPoints != TotalPointsPerGame)
            {
                throw new InvalidOperationException(
                    "Team B can only receive match bonus when winning all points.");
            }

            if (teamAMatchBonusAchieved && teamBMatchBonusAchieved)
            {
                throw new InvalidOperationException(
                    "Only one team can receive the match bonus.");
            }

            Guard.AgainstEmptyGuid(enteredBy, nameof(enteredBy));
        }

        private static int ApplyMatchBonus(int points, bool matchBonusAchieved)
        {
            return matchBonusAchieved
                ? points + MatchBonusPoints
                : points;
        }
    }
}
