using JassTournamentManager.Application.Auth;
using JassTournamentManager.Application.TournamentTemplates;
using Microsoft.Extensions.DependencyInjection;

namespace JassTournamentManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITournamentTemplateService, TournamentTemplateService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
