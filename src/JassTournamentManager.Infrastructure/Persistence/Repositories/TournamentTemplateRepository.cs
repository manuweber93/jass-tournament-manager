using JassTournamentManager.Application.TournamentTemplates;
using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JassTournamentManager.Infrastructure.Persistence.Repositories
{
    public sealed class TournamentTemplateRepository : ITournamentTemplateRepository
    {
        private readonly JtmDbContext _dbContext;

        public TournamentTemplateRepository(JtmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<TournamentTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.TournamentTemplates.SingleOrDefaultAsync(tournamentTemplate => tournamentTemplate.Id == id, cancellationToken);
        }

        public Task<TournamentTemplate?> GetByOrganizerIdAsync(Guid organizerId, CancellationToken cancellationToken)
        {
            return _dbContext.TournamentTemplates.SingleOrDefaultAsync(tournamentTemplate => tournamentTemplate.OrganizerId == organizerId, cancellationToken);
        }

        public async Task AddAsync(TournamentTemplate tournamentTemplate, CancellationToken cancellationToken)
        {
            await _dbContext.TournamentTemplates.AddAsync(tournamentTemplate, cancellationToken);
        }

        public Task<bool> ExistsForOrganizerAsync(Guid organizerId, CancellationToken cancellationToken)
        {
            return _dbContext.TournamentTemplates.AnyAsync(tournamentTemplate => tournamentTemplate.OrganizerId == organizerId, cancellationToken);
        }

    }
}
