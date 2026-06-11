using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Auth
{
    public interface IUserPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(string passwordhash, string providedPassword);
    }
}
