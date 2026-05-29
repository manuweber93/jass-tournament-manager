using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Users
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken);
    }
}
