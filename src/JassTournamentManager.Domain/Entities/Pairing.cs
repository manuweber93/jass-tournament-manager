using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.Entities
{
    public class Pairing : BaseEntity
    {
        private const PairingStatus DefaultPairingStatus = PairingStatus.Pending;

        private readonly List<Game> _games = [];

        public Guid RoundId { get; private set; }

        public Guid JassTableId { get; private set; }

        public PairingStatus Status { get; private set; }

        public IReadOnlyCollection<Game> Games => _games.AsReadOnly();

        private Pairing() { }

        public Pairing(Guid roundId, Guid jassTableId, PairingStatus status = DefaultPairingStatus)
        {
            Guard.AgainstEmptyGuid(roundId, nameof(roundId));
            Guard.AgainstEmptyGuid(jassTableId, nameof(jassTableId));

            RoundId = roundId;
            JassTableId = jassTableId;
            Status = status;
        }

        public void AddGame(Game game, int gamesPerRound)
        {
            if (game.GameNumber > gamesPerRound)
            {
                throw new InvalidOperationException($"Game number {game.GameNumber} exceeds the configured number of games per round ({gamesPerRound}).");
            }
            
            _games.Add(game);
        }

        public void Complete()
        {
            Status = PairingStatus.Completed;
            MarkAsUpdated();
        }

        public void SetBackToPending()
        {
            Status = PairingStatus.Pending;
            MarkAsUpdated();
        }
    }
}
