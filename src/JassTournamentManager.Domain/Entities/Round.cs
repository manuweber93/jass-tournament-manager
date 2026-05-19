using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.Entities
{
    public class Round : BaseEntity
    {
        private const RoundStatus DefaultRoundStatus = RoundStatus.Pending;

        private readonly List<Pairing> _pairings = [];

        public Guid TournamentId { get; private set; }

        public int RoundNumber { get; private set; }

        public RoundStatus Status { get; private set; }

        public IReadOnlyCollection<Pairing> Pairings => _pairings.AsReadOnly();

        private Round() { }

        public Round(Guid tournamentId, int roundNumber, RoundStatus status = DefaultRoundStatus)
        {
            Guard.AgainstEmptyGuid(tournamentId, nameof(tournamentId));
            ArgumentOutOfRangeException.ThrowIfLessThan(roundNumber, 1);

            TournamentId = tournamentId;
            RoundNumber = roundNumber;
            Status = status;
        }

        public void AddPairing(Pairing pairing)
        {
            ArgumentNullException.ThrowIfNull(pairing);

            if (pairing.RoundId != Id)
            {
                throw new InvalidOperationException("Pairing belongs to a different round.");
            }

            if (_pairings.Any(p => p.JassTableId == pairing.JassTableId))
            {
                throw new InvalidOperationException("Jass table is already assigned in this round.");
            }

            _pairings.Add(pairing);
            MarkAsUpdated();
        }

        public void Activate()
        {
            Status = RoundStatus.Active;
            MarkAsUpdated();
        }

        public void Complete()
        {
            Status = RoundStatus.Completed;
            MarkAsUpdated();
        }

        public void SetBackToPending()
        {
            Status = RoundStatus.Pending;
            MarkAsUpdated();
        }

        public void UpdateMatchBonusEnabledForGames(bool matchBonusEnabled)
        {
            foreach (var pairing in _pairings)
            {
                pairing.UpdateMatchBonusEnabledForGames(matchBonusEnabled);
            }

            MarkAsUpdated();
        }
    }
}
