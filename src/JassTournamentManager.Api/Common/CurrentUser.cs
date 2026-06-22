using JassTournamentManager.Application.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JassTournamentManager.Api.Common
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

        public Guid UserId
        {
            get
            {
                string? value = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

                if (!Guid.TryParse(value, out Guid userId))
                {
                    throw new InvalidOperationException("Authenticated user id claim is missing or invalid.");
                }

                return userId;
            }
        }

        public string? Email => User?.FindFirstValue(ClaimTypes.Email);

        public bool IsSysAdmin => string.Equals(User?.FindFirstValue("is_sys_admin"), "true", StringComparison.OrdinalIgnoreCase);
    }
}
