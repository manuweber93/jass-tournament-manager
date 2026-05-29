using JassTournamentManager.Application.TournamentTemplates;
using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Tests.TournamentTemplates
{
    internal sealed class FakeTournamentTemplateRepository : ITournamentTemplateRepository
    {
        private readonly List<TournamentTemplate> _tournamentTemplates = [];

        public IReadOnlyCollection<TournamentTemplate> TournamentTemplates => _tournamentTemplates.AsReadOnly();

        public Task AddAsync(TournamentTemplate tournamentTemplate, CancellationToken cancellationToken)
        {
            _tournamentTemplates.Add(tournamentTemplate);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsForOrganizerAsync(Guid organizerId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_tournamentTemplates.Any(tournamentTemplate => tournamentTemplate.OrganizerId == organizerId));
        }

        public Task<TournamentTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(_tournamentTemplates.SingleOrDefault(tournamentTemplate => tournamentTemplate.Id == id));
        }
    }
}
