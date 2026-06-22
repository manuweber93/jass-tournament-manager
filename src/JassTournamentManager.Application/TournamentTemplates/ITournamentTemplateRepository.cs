using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.TournamentTemplates
{
    public interface ITournamentTemplateRepository
    {
        Task<TournamentTemplate?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<TournamentTemplate?> GetByOrganizerIdAsync(Guid organizerId, CancellationToken cancellationToken);

        Task AddAsync(TournamentTemplate tournamentTemplate, CancellationToken cancellationToken);

        Task<bool> ExistsForOrganizerAsync(Guid organizerId, CancellationToken cancellationToken);
    }
}
