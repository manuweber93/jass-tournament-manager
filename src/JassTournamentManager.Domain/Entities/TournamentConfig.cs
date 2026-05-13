using JassTournamentManager.Domain.Common;

namespace JassTournamentManager.Domain.Entities
{
    public class TournamentConfig : BaseEntity
    {
        public Guid TournamentId { get; private set; }

        public Guid? TournamentConfigTemplateId { get; private set; } = null;

        public TournamentConfigValues ConfigValues { get; private set; } = null!;

        private TournamentConfig() { }

        public TournamentConfig(Guid tournamentId, Guid? tournamentConfigTemplateId, TournamentConfigValues configValues)
        {
            Guard.AgainstEmptyGuid(tournamentId, nameof(tournamentId));

            TournamentId = tournamentId;
            TournamentConfigTemplateId = tournamentConfigTemplateId;
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
