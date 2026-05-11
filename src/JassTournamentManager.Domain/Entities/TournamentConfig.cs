using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

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
            if (tournamentId == Guid.Empty)
            {
                throw new ArgumentException("Tournament ID must not be empty.", nameof(tournamentId));
            }

            TournamentId = tournamentId;
            TournamentConfigTemplateId = tournamentConfigTemplateId;
            ConfigValues = configValues ?? throw new ArgumentNullException(nameof(configValues));
        }

        public void UpdateConfig(TournamentConfigValues configValues)
        {
            ConfigValues = configValues ?? throw new ArgumentNullException(nameof(configValues));
            MarkAsUpdated();
        }
    }
}
