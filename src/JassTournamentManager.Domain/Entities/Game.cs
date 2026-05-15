using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Entities
{
    public class Game : BaseEntity
    {
        private const GameStatus DefaultGameStatus = GameStatus.Pending;

        public Guid PairingId { get; private set; }

        public int GameNumber { get; private set; }

        public GameStatus Status { get; private set; }

        public GameScore? Score { get; private set; }

        private Game() { }

        public Game(Guid pairingId, int gameNumber, GameStatus status = DefaultGameStatus)
        {
            Guard.AgainstEmptyGuid(pairingId, nameof(pairingId));
            ArgumentOutOfRangeException.ThrowIfLessThan(gameNumber, 1);

            PairingId = pairingId;
            GameNumber = gameNumber;
            Status = status;
        }

        public void SetScore(GameScore score)
        {
            ArgumentNullException.ThrowIfNull(score);
            Score = score;
            MarkAsUpdated();
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
