using JassTournamentManager.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace JassTournamentManager.Infrastructure.Tests.Persistence
{
    [CollectionDefinition(Name, DisableParallelization = true)]
    public sealed class PostgreSqlCollection : ICollectionFixture<PostgreSqlFixture>
    {
        public const string Name = "PostgreSql";
    }

    public sealed class PostgreSqlFixture : IAsyncLifetime
    {
        private PostgreSqlContainer? _container;

        public string ConnectionString => _container?.GetConnectionString()
            ?? throw new InvalidOperationException("PostgreSQL test container has not been initialized.");

        public async Task InitializeAsync()
        {
            try
            {
                _container = new PostgreSqlBuilder()
                    .WithImage("postgres:16-alpine")
                    .WithDatabase("jass_tournament_manager_tests")
                    .WithUsername("postgres")
                    .WithPassword("postgres")
                    .Build();

                await _container.StartAsync();
            }
            catch
            {
                _container = null;
                throw;
            }
        }

        public async Task DisposeAsync()
        {
            if (_container is not null)
            {
                await _container.DisposeAsync();
            }
        }

        public JtmDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<JtmDbContext>()
                .UseNpgsql(ConnectionString)
                .Options;

            return new JtmDbContext(options);
        }

        public async Task ResetDatabaseAsync()
        {
            await using var dbContext = CreateDbContext();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
        }
    }
}
