using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JassTournamentManager.Api.Models;

namespace JassTournamentManager.Api.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets for domain entities can be added here, e.g.
    // public DbSet<Tournament> Tournaments { get; set; }
}
