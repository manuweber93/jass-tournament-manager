using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JassTournamentManager.Infrastructure.Tests.Persistence
{
    [Collection(PostgreSqlCollection.Name)]
    public class DbContextModelTests
    {
        private readonly PostgreSqlFixture _fixture;

        public DbContextModelTests(PostgreSqlFixture fixture)
        {
            _fixture = fixture;
        }

        [DockerAvailableFact]
        public async Task Can_Create_Database_Model()
        {
            await _fixture.ResetDatabaseAsync();

            await using var dbContext = _fixture.CreateDbContext();

            dbContext.Model.FindEntityType(typeof(User)).Should().NotBeNull();
            dbContext.Model.FindEntityType(typeof(Tournament)).Should().NotBeNull();
            dbContext.Model.FindEntityType(typeof(TournamentTemplate)).Should().NotBeNull();
            dbContext.Model.FindEntityType(typeof(TournamentParticipant)).Should().NotBeNull();
            dbContext.Model.FindEntityType(typeof(Round)).Should().NotBeNull();
            dbContext.Model.FindEntityType(typeof(JassTable)).Should().NotBeNull();
            dbContext.Model.FindEntityType(typeof(Pairing)).Should().NotBeNull();
            dbContext.Model.FindEntityType(typeof(PairingParticipant)).Should().NotBeNull();
            dbContext.Model.FindEntityType(typeof(Game)).Should().NotBeNull();
        }

        [DockerAvailableFact]
        public async Task DbSets_Can_Query_All_Configured_Entities()
        {
            await _fixture.ResetDatabaseAsync();

            await using var dbContext = _fixture.CreateDbContext();

            (await dbContext.Users.CountAsync()).Should().Be(0);
            (await dbContext.TournamentTemplates.CountAsync()).Should().Be(0);
            (await dbContext.JassTables.CountAsync()).Should().Be(0);
            (await dbContext.Tournaments.CountAsync()).Should().Be(0);
            (await dbContext.TournamentParticipants.CountAsync()).Should().Be(0);
            (await dbContext.Rounds.CountAsync()).Should().Be(0);
            (await dbContext.Pairings.CountAsync()).Should().Be(0);
            (await dbContext.PairingParticipants.CountAsync()).Should().Be(0);
            (await dbContext.Games.CountAsync()).Should().Be(0);
        }
    }
}
