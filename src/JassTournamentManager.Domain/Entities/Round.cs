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

        public Guid TournamentId { get; private set; }

        public Tournament Tournament { get; private set; } = null!;

        public int RoundNumber { get; private set; }

        public RoundStatus Status { get; private set; }

        private Round() { }

        public Round(Tournament tournament, int roundNumber, RoundStatus status = DefaultRoundStatus)
        {
            ArgumentNullException.ThrowIfNull(tournament);

            if (roundNumber < 1 || roundNumber > tournament.Config.ConfigValues.NumberOfRounds)
            {
                throw new ArgumentOutOfRangeException(nameof(roundNumber), "Round number must be greater than null and not greater than the number of rounds of the tournament");
            }

            Tournament = tournament;
            TournamentId = tournament.Id;
            RoundNumber = roundNumber;
            Status = status;
        }
    }
}
