using JassTournamentManager.Application.Auth;
using JassTournamentManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace JassTournamentManager.Infrastructure.Auth
{
    public class UserPasswordHasher : IUserPasswordHasher
    {
        private readonly PasswordHasher<object> _passwordHasher = new();

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(new object(), password);
        }

        public bool VerifyPassword(string passwordhash, string providedPassword)
        {
            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(new object(), passwordhash, providedPassword);
            return verificationResult is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
