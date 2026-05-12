using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JassTournamentManager.Domain.Entities
{
    public class PairingParticipant : BaseEntity
    {
        public Guid PairingId { get; private set; }

        public Guid TournamentParticipantId { get; private set; }

        public Team Team { get; private set; }

        public Guid? EnteredBy { get; private set; }

        private PairingParticipant() { }

        public PairingParticipant(Guid pairingId, Guid tournamentParticipantId, Team team, Guid? enteredBy)
        {
            if (pairingId == Guid.Empty)
            {
                throw new ArgumentException("Pairing id must not be empty.", nameof(pairingId));
            }

            if (tournamentParticipantId == Guid.Empty)
            {
                throw new ArgumentException("Tournament participant id must not be empty.", nameof(tournamentParticipantId));
            }

            PairingId = pairingId;
            TournamentParticipantId = tournamentParticipantId;
            Team = team;
            EnteredBy = enteredBy;
        }

        public void UpdateTournamentParticipantId(Guid newTournamentParticipantId)
        {
            if (newTournamentParticipantId == Guid.Empty)
            {
                throw new ArgumentException("New tournament participant id must not be empty.", nameof(newTournamentParticipantId));
            }

            TournamentParticipantId = newTournamentParticipantId;
            MarkAsUpdated();
        }

        public void UpdateTeam(Team team)
        {
            Team = team;
            MarkAsUpdated();
        }
    }
}
