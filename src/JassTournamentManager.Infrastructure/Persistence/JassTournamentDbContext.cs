using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JassTournamentManager.Infrastructure.Persistence
{
    public sealed class JassTournamentDbContext : DbContext
    {
        public JassTournamentDbContext(DbContextOptions<JassTournamentDbContext> options) : base(options)
        {
        }

        public DbSet<Tournament> Tournaments => Set<Tournament>();

        public DbSet<Round> Rounds => Set<Round>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("jtm");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(JassTournamentDbContext).Assembly);
        }
    }
}
