using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Auth
{
    public interface IUserPasswordHasher
    {
        string HashPassword(User user,  string password);

        bool VerifyPassword(User user, string passwordhash, string password);
    }
}
