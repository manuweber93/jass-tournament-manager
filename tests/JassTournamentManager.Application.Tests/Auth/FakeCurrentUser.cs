using JassTournamentManager.Application.Auth;

namespace JassTournamentManager.Application.Tests.Auth
{
    internal sealed class FakeCurrentUser : ICurrentUser
    {
        public bool IsAuthenticated { get; set; } = true;

        public Guid UserId { get; set; }

        public string? Email { get; set; }

        public bool IsSysAdmin { get; set; } = false;
    }
}
