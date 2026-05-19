using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Entities
{
    public class TournamentTemplate : BaseEntity
    {
        public Guid OrganizerId { get; private set; }

        public TournamentConfigValues ConfigValues { get; private set; } = null!;

        public string? Location { get; private set; }

        private TournamentTemplate() { }

        public TournamentTemplate(Guid organizerId, TournamentConfigValues configValues, string? location)
        {
            Guard.AgainstEmptyGuid(organizerId, nameof(organizerId));

            OrganizerId = organizerId;
            ConfigValues = configValues ?? throw new ArgumentNullException(nameof(configValues));
            Location = location?.Trim();
        }

        public void UpdateLocation(string? location)
        {
            Location = location?.Trim();
            MarkAsUpdated();
        }

        public void UpdateConfig(TournamentConfigValues configValues)
        {
            ArgumentNullException.ThrowIfNull(configValues);

            ConfigValues = configValues;
            MarkAsUpdated();
        }
    }
}
