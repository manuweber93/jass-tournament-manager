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
            Guard.AgainstEmptyGuid(organizerId, nameof(organizerId));

            OrganizerId = organizerId;
            ConfigValues = configValues ?? throw new ArgumentNullException(nameof(configValues));
        }

        public void UpdateConfig(TournamentConfigValues configValues)
        {
            TournamentConfigValues.ValidateConfigValues(configValues);
            ConfigValues = configValues;
            MarkAsUpdated();
        }
    }
}
