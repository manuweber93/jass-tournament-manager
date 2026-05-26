using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.Entities
{
    public class PairingParticipant : BaseEntity
    {
        public Guid PairingId { get; private set; }

        public Guid TournamentParticipantId { get; private set; }

        public Team Team { get; private set; }

        public Guid EnteredByUserId { get; private set; }

        private PairingParticipant() { }

        public PairingParticipant(Guid pairingId, Guid tournamentParticipantId, Team team, Guid enteredByUserId)
        {
            Guard.AgainstEmptyGuid(pairingId, nameof(pairingId));
            Guard.AgainstEmptyGuid(tournamentParticipantId, nameof(tournamentParticipantId));
            Guard.AgainstEmptyGuid(enteredByUserId, nameof(enteredByUserId));

            PairingId = pairingId;
            TournamentParticipantId = tournamentParticipantId;
            Team = team;
            EnteredByUserId = enteredByUserId;
        }

        public void UpdateTournamentParticipant(Guid newTournamentParticipantId)
        {
            Guard.AgainstEmptyGuid(newTournamentParticipantId, nameof(newTournamentParticipantId));

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
