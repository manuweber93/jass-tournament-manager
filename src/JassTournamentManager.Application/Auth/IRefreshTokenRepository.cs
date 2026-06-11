using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Auth
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);

        Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken);
    }
}
