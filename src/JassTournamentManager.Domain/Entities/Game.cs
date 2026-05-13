using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.Entities
{
    public class Game : BaseEntity
    {
        private const GameStatus DefaultGameStatus = GameStatus.Pending;
        private const int TotalPointsPerGame = 157;

        public Guid PairingId { get; private set; }

        public int GameNumber { get; private set; }

        public GameStatus Status { get; private set; }

        public int? TeamAPoints { get; private set; }

        public int? TeamBPoints { get; private set; }

        public bool TeamAMatchBonusAchieved { get; private set; }

        public bool TeamBMatchBonusAchieved { get; private set; }

        public Guid? EnteredBy { get; private set; }

        public DateTimeOffset? EnteredAt { get; private set; }


        private Game() { }

        public Game(Guid pairingId, int gameNumber, GameStatus status = DefaultGameStatus)
        {
            Guard.AgainstEmptyGuid(pairingId, nameof(pairingId));
            ArgumentOutOfRangeException.ThrowIfLessThan(gameNumber, 1);

            PairingId = pairingId;
            GameNumber = gameNumber;
            Status = status;
        }

        public void UpdateScoreForTeam(Team team, int points, bool matchBonusAchieved, bool opponentMatchBonusAchieved, Guid? enteredBy)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(points);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(points, TotalPointsPerGame);

            if (matchBonusAchieved && opponentMatchBonusAchieved)
            {
                throw new ArgumentException("Only one team can receive the match bonus.", nameof(matchBonusAchieved));
            }

            int pointsOfOpponents = TotalPointsPerGame - points;

            if (team == Team.TeamA)
            {
                SetPointsAndMatchBonus(points, matchBonusAchieved, v => TeamAPoints = v, v => TeamAMatchBonusAchieved = v);
                SetPointsAndMatchBonus(pointsOfOpponents, opponentMatchBonusAchieved, v => TeamBPoints = v, v => TeamBMatchBonusAchieved = v);
            } else if (team == Team.TeamB) {
                SetPointsAndMatchBonus(points, matchBonusAchieved, v => TeamBPoints = v, v => TeamBMatchBonusAchieved = v);
                SetPointsAndMatchBonus(pointsOfOpponents, opponentMatchBonusAchieved, v => TeamAPoints = v, v => TeamAMatchBonusAchieved = v);
            } else
            {
                throw new InvalidOperationException("Invalid team passed.");
            }

            EnteredBy = enteredBy;
            EnteredAt = DateTimeOffset.UtcNow;
            MarkAsUpdated();
        }

        private static void SetPointsAndMatchBonus(int points, bool matchBonusReceived, Action<int> pointsSetter, Action<bool> matchBonusSetter)
        {
            pointsSetter(points);
            matchBonusSetter(matchBonusReceived);
        }

        public void Complete()
        {
            Status = GameStatus.Completed;
            MarkAsUpdated();
        }

        public void SetBackToPending()
        {
            Status = GameStatus.Pending;
            MarkAsUpdated();
        }
    }
}
