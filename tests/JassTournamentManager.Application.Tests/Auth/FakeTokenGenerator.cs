using JassTournamentManager.Application.Auth;
using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Tests.Auth
{
    internal sealed class FakeTokenGenerator : ITokenGenerator
    {
        public string AccessToken { get; set; } = "access-token";
        public DateTimeOffset AccessTokenExpiresAt { get; set; } =
            new(2043, 11, 30, 1, 0, 0, TimeSpan.Zero);

        public string RefreshToken { get; set; } = "refresh-token";
        public string RefreshTokenHash { get; set; } = "refresh-token-hash";
        public DateTimeOffset RefreshTokenExpiresAt { get; set; } =
            new(2043, 11, 30, 2, 0, 0, TimeSpan.Zero);

        public AccessTokenSecret GenerateAccessTokenSecret(User user) =>
            new(AccessToken, AccessTokenExpiresAt);

        public RefreshTokenSecret GenerateRefreshTokenSecret() =>
            new(RefreshToken, RefreshTokenHash, RefreshTokenExpiresAt);

        public string HashRefreshToken(string refreshToken) =>
            refreshToken == RefreshToken ? RefreshTokenHash : $"hash:{refreshToken}";
    }
}
