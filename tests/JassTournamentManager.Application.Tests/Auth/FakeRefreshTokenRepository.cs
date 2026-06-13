using JassTournamentManager.Application.Auth;
using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Tests.Auth
{
    internal sealed class FakeRefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly List<RefreshToken> _refreshTokens = [];

        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            _refreshTokens.Add(refreshToken);
            return Task.CompletedTask;
        }

        public Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken)
        {
            return Task.FromResult(_refreshTokens.SingleOrDefault(refreshToken => refreshToken.TokenHash == tokenHash));
        }
    }
}
