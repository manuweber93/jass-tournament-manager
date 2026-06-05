using JassTournamentManager.Application.Auth;
using JassTournamentManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace JassTournamentManager.Infrastructure.Auth
{
    public class UserPasswordHasher : IUserPasswordHasher
    {
        private readonly PasswordHasher<User> _passwordHasher = new();

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string passwordhash, string password)
        {
            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, passwordhash, password);
            return verificationResult is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
