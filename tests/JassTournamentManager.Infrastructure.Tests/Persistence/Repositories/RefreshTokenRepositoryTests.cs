using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JassTournamentManager.Infrastructure.Tests.Persistence.Repositories
{
    [Collection(PostgreSqlCollection.Name)]
    public class RefreshTokenRepositoryTests
    {
        private readonly PostgreSqlFixture _fixture;

        public RefreshTokenRepositoryTests(PostgreSqlFixture fixture)
        {
            _fixture = fixture;
        }

        [DockerAvailableFact]
        public async Task AddAsync_PersistsRefreshToken()
        {
            await _fixture.ResetDatabaseAsync();

            var user = PersistenceTestData.CreateUser();
            var refreshToken = PersistenceTestData.CreateRefreshToken(user.Id);

            await PersistRefreshToken(user, refreshToken);

            await using var assertionContext = _fixture.CreateDbContext();
            var addedRefreshToken = await assertionContext.RefreshTokens
                .SingleAsync(token => token.Id == refreshToken.Id);

            VerifyRefreshToken(user, refreshToken, addedRefreshToken);
        }

        [DockerAvailableFact]
        public async Task GetByHashAsync_WithExistingHash_ReturnsRefreshToken()
        {
            await _fixture.ResetDatabaseAsync();

            var user = PersistenceTestData.CreateUser();
            var refreshToken = PersistenceTestData.CreateRefreshToken(user.Id);

            await PersistRefreshToken(user, refreshToken);

            await using var assertionContext = _fixture.CreateDbContext();
            var refreshTokenRepository = new RefreshTokenRepository(assertionContext);
            var loadedRefreshToken = await refreshTokenRepository.GetByHashAsync(refreshToken.TokenHash, CancellationToken.None);

            VerifyRefreshToken(user, loadedRefreshToken, refreshToken);
        }

        [DockerAvailableFact]
        public async Task GetByHashAsync_WithoutExistingHash_ReturnsNull()
        {
            await _fixture.ResetDatabaseAsync();

            var missingRefreshTokenHash = "abc";

            await using var assertionContext = _fixture.CreateDbContext();
            var refreshTokenRepository = new RefreshTokenRepository(assertionContext);
            var loadedRefreshToken = await refreshTokenRepository.GetByHashAsync(missingRefreshTokenHash, CancellationToken.None);

            loadedRefreshToken.Should().BeNull();
        }

        private async Task PersistRefreshToken(User user, RefreshToken refreshToken)
        {
            await using var dbContext = _fixture.CreateDbContext();
            var refreshTokenRepository = new RefreshTokenRepository(dbContext);

            dbContext.Users.Add(user);
            await refreshTokenRepository.AddAsync(refreshToken, CancellationToken.None);
            await dbContext.SaveChangesAsync();
        }

        private static void VerifyRefreshToken(User user, RefreshToken? refreshToken, RefreshToken refreshTokenToVerifyAgainst)
        {
            refreshToken.Should().NotBeNull();

            var persistedRefreshToken = refreshToken!;
            persistedRefreshToken.Id.Should().Be(refreshTokenToVerifyAgainst.Id);
            persistedRefreshToken.UserId.Should().Be(user.Id);
            persistedRefreshToken.TokenHash.Should().Be(refreshTokenToVerifyAgainst.TokenHash);
            persistedRefreshToken.ExpiresAtUtc.Should().Be(refreshTokenToVerifyAgainst.ExpiresAtUtc);
        }
    }
}
