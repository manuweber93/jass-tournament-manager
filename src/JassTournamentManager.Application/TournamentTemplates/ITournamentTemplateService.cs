using JassTournamentManager.Application.Auth;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Contracts.TournamentTemplates;

namespace JassTournamentManager.Application.TournamentTemplates
{
    public interface ITournamentTemplateService
    {
        Task<Result<TournamentTemplateResponse>> CreateAsync(CreateTournamentTemplateRequest request, CancellationToken cancellationToken);

        Task<Result<TournamentTemplateResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<Result<TournamentTemplateResponse>> GetForCurrentUserAsync(CancellationToken cancellationToken);
    }
}