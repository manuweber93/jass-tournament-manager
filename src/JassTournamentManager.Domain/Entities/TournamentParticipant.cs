using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.Entities
{
    public class TournamentParticipant : BaseEntity
    {
        private const ParticipantRole DefaultParticipantRole = ParticipantRole.Player;
        private const bool DefaultIsPlaying = true;

        public Guid TournamentId { get; private set; }

        public Guid UserId { get; private set; }

        public RegistrationMethod RegistrationMethod { get; private set; }

        public ParticipantRole Role { get; private set; }

        public bool IsPlaying { get; private set; }

        public DateTimeOffset RegisteredAt { get; private set; }

        private TournamentParticipant() { }

        public TournamentParticipant(
            Guid tournamentId,
            Guid userId,
            RegistrationMethod registrationMethod,
            ParticipantRole role = DefaultParticipantRole,
            bool isPlaying = DefaultIsPlaying)
        {
            Guard.AgainstEmptyGuid(tournamentId, nameof(tournamentId));
            Guard.AgainstEmptyGuid(userId, nameof(userId));

            TournamentId = tournamentId;
            UserId = userId;
            Role = role;
            IsPlaying = isPlaying;
            RegistrationMethod = registrationMethod;
            RegisteredAt = DateTimeOffset.UtcNow;
        }

        public void UpdateDetails(ParticipantRole role, bool isPlaying)
        {
            Role = role;
            IsPlaying = isPlaying;
            MarkAsUpdated();
        }
    }
}
