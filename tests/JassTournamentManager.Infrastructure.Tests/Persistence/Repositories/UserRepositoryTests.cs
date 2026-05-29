using FluentAssertions;
using JassTournamentManager.Infrastructure.Persistence.Repositories;

namespace JassTournamentManager.Infrastructure.Tests.Persistence.Repositories
{
    [Collection(PostgreSqlCollection.Name)]
    public class UserRepositoryTests
    {
        private readonly PostgreSqlFixture _fixture;

        public UserRepositoryTests(PostgreSqlFixture fixture)
        {
            _fixture = fixture;
        }

        [DockerAvailableFact]
        public async Task ExistsAsync_WithExistentUser_ReturnsTrue()
        {
            await _fixture.ResetDatabaseAsync();
            var user = PersistenceTestData.CreateUser();

            await using (var dbContext = _fixture.CreateDbContext())
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            await using var assertionContext = _fixture.CreateDbContext();
            var repository = new UserRepository(assertionContext);

            var exists = await repository.ExistsAsync(user.Id, CancellationToken.None);

            exists.Should().BeTrue();
        }

        [DockerAvailableFact]
        public async Task ExistsAsync_WithNonExistentUser_ReturnsFalse()
        {
            await _fixture.ResetDatabaseAsync();
            var missingUserId = Guid.NewGuid();

            await using var assertionContext = _fixture.CreateDbContext();
            var repository = new UserRepository(assertionContext);

            var exists = await repository.ExistsAsync(missingUserId, CancellationToken.None);
            
            exists.Should().BeFalse();
        }
    }
}
