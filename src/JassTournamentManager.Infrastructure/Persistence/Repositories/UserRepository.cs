using JassTournamentManager.Application.Users;
using JassTournamentManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JassTournamentManager.Infrastructure.Persistence.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly JtmDbContext _dbContext;

        public UserRepository(JtmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }

        public Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _dbContext.Users.AnyAsync(user => user.Id == userId, cancellationToken);
        }
    }
}
