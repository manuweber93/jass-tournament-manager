using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

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
            if (tournamentId == Guid.Empty)
            {
                throw new ArgumentException("Tournament id must not be empty.", nameof(tournamentId));
            }

            ArgumentOutOfRangeException.ThrowIfLessThan(roundNumber, 1);

            TournamentId = tournamentId;
            RoundNumber = roundNumber;
            Status = status;
        }

        public void AddPairing(Pairing pairing)
        {
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
    }
}
