using JassTournamentManager.Api.Common;
using JassTournamentManager.Application.Auth;

namespace JassTournamentManager.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUser, CurrentUser>();

            return services;
        }
    }
}
