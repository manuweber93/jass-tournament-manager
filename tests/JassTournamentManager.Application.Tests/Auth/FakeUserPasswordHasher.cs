using JassTournamentManager.Application.Auth;

namespace JassTournamentManager.Application.Tests.Auth
{
    internal sealed class FakeUserPasswordHasher : IUserPasswordHasher
    {
        public string PasswordHash { get; set; } = "hashed-password";
        public bool IsPasswordValid { get; set; } = true;

        public string HashPassword(string password) => PasswordHash;

        public bool VerifyPassword(string passwordHash, string providedPassword) =>
            IsPasswordValid;
    }
}
