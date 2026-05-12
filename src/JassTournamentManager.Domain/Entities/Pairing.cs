using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

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
            if (roundId == Guid.Empty)
            {
                throw new ArgumentException("Round id must not be empty.", nameof(roundId));
            }

            if (jassTableId == Guid.Empty)
            {
                throw new ArgumentException("Jass table id must not be empty.", nameof(jassTableId));
            }

            RoundId = roundId;
            JassTableId = jassTableId;
            Status = status;
        }

        public void AddGame(Game game, int gamesPerRound)
        {
            // TODO: validate against tournament config
            
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
