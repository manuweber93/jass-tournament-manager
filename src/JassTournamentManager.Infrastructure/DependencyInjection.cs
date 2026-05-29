using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.TournamentTemplates;
using JassTournamentManager.Application.Users;
using JassTournamentManager.Infrastructure.Persistence;
using JassTournamentManager.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JassTournamentManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<JtmDbContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "jtm")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITournamentTemplateRepository, TournamentTemplateRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
