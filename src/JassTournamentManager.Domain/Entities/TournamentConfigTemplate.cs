using JassTournamentManager.Domain.Common;

namespace JassTournamentManager.Domain.Entities
{
    public class TournamentConfigTemplate : BaseEntity
    {
        public Guid OrganizerId { get; private set; }

        public TournamentConfigValues ConfigValues { get; private set; } = null!;

        private TournamentConfigTemplate() { }

        public TournamentConfigTemplate(Guid organizerId, TournamentConfigValues configValues)
        {
            if (organizerId == Guid.Empty)
            {
                throw new ArgumentException("Organizer ID must not be empty.", nameof(organizerId));
            }

            OrganizerId = organizerId;
            ConfigValues = configValues ?? throw new ArgumentNullException(nameof(configValues));
        }

        public void UpdateConfig(TournamentConfigValues configValues)
        {
            ConfigValues = configValues ?? throw new ArgumentNullException(nameof(configValues));
            MarkAsUpdated();
        }
    }
}
