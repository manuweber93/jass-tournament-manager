using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JassTournamentManager.Infrastructure.Tests.Persistence.Repositories
{
    [Collection(PostgreSqlCollection.Name)]
    public class TournamentTemplateRepositoryTests
    {
        private readonly PostgreSqlFixture _fixture;

        public TournamentTemplateRepositoryTests(PostgreSqlFixture fixture)
        {
            _fixture = fixture;
        }

        [DockerAvailableFact]
        public async Task AddAsync_PersistsTournamentTemplate()
        {
            await _fixture.ResetDatabaseAsync();

            var organizer = PersistenceTestData.CreateUser();
            var tournamentTemplate = PersistenceTestData.CreateTournamentTemplate(organizer.Id);

            await PersistTournamentTemplate(organizer, tournamentTemplate);

            await using var assertionContext = _fixture.CreateDbContext();
            var addedTournamentTemplate = await assertionContext.TournamentTemplates
                .SingleAsync(template => template.Id == tournamentTemplate.Id, CancellationToken.None);

            VerifyTournamentTemplate(organizer, addedTournamentTemplate, tournamentTemplate);
        }

        [DockerAvailableFact]
        public async Task ExistsForOrganizerAsync_WithExistingTournamentTemplateForOrganizer_ReturnsTrue()
        {
            await _fixture.ResetDatabaseAsync();

            var organizer = PersistenceTestData.CreateUser();
            var tournamentTemplate = PersistenceTestData.CreateTournamentTemplate(organizer.Id);

            await PersistTournamentTemplate(organizer, tournamentTemplate);

            await using var assertionContext = _fixture.CreateDbContext();
            var tournamentTemplateRepository = new TournamentTemplateRepository(assertionContext);
            var exists = await tournamentTemplateRepository.ExistsForOrganizerAsync(organizer.Id, CancellationToken.None);

            exists.Should().BeTrue();
        }

        [DockerAvailableFact]
        public async Task ExistsForOrganizerAsync_WithoutExistingTournamentTemplateForOrganizer_ReturnsFalse()
        {
            await _fixture.ResetDatabaseAsync();

            var organizer = PersistenceTestData.CreateUser();

            await using var assertionContext = _fixture.CreateDbContext();
            var tournamentTemplateRepository = new TournamentTemplateRepository(assertionContext);
            var exists = await tournamentTemplateRepository.ExistsForOrganizerAsync(organizer.Id, CancellationToken.None);

            exists.Should().BeFalse();
        }

        [DockerAvailableFact]
        public async Task GetByIdAsync_WithExistingTournamentTemplate_ReturnsTournamentTemplate()
        {
            await _fixture.ResetDatabaseAsync();

            var organizer = PersistenceTestData.CreateUser();
            var tournamentTemplate = PersistenceTestData.CreateTournamentTemplate(organizer.Id);

            await PersistTournamentTemplate(organizer, tournamentTemplate);

            await using var assertionContext = _fixture.CreateDbContext();
            var tournamentTemplateRepository = new TournamentTemplateRepository(assertionContext);
            var loadedTournamentTemplate = await tournamentTemplateRepository.GetByIdAsync(tournamentTemplate.Id, CancellationToken.None);

            VerifyTournamentTemplate(organizer, loadedTournamentTemplate, tournamentTemplate);
        }

        [DockerAvailableFact]
        public async Task GetByIdAsync_WithoutExistingTournamentTemplate_ReturnsNull()
        {
            await _fixture.ResetDatabaseAsync();

            var missingTournamentTemplateId = Guid.NewGuid();

            await using var assertionContext = _fixture.CreateDbContext();
            var tournamentTemplateRepository = new TournamentTemplateRepository(assertionContext);
            var loadedTournamentTemplate = await tournamentTemplateRepository.GetByIdAsync(missingTournamentTemplateId, CancellationToken.None);

            loadedTournamentTemplate.Should().BeNull();
        }

        private async Task PersistTournamentTemplate(User organizer, TournamentTemplate tournamentTemplate)
        {
            await using var dbContext = _fixture.CreateDbContext();
            var tournamentTemplateRepository = new TournamentTemplateRepository(dbContext);

            await dbContext.Users.AddAsync(organizer, CancellationToken.None);
            await tournamentTemplateRepository.AddAsync(tournamentTemplate, CancellationToken.None);
            await dbContext.SaveChangesAsync();
        }

        private static void VerifyTournamentTemplate(User organizer, TournamentTemplate? tournamentTemplate, TournamentTemplate tournamentTemplateToVerifyAgainst)
        {
            tournamentTemplate.Should().NotBeNull();

            var persistedTournamentTemplate = tournamentTemplate!;
            persistedTournamentTemplate.Id.Should().Be(tournamentTemplateToVerifyAgainst.Id);
            persistedTournamentTemplate.OrganizerId.Should().Be(organizer.Id);
            persistedTournamentTemplate.Location.Should().Be(tournamentTemplateToVerifyAgainst.Location);
            persistedTournamentTemplate.ConfigValues.Should().BeEquivalentTo(tournamentTemplateToVerifyAgainst.ConfigValues);
        }
    }
}
