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

        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Users.SingleOrDefaultAsync(user => user.Id == id, cancellationToken);
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return _dbContext.Users.SingleOrDefaultAsync(user => user.Email == email.Trim().ToLowerInvariant(), cancellationToken);
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return _dbContext.Users.AnyAsync(user => user.Id == id, cancellationToken);
        }

        public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return _dbContext.Users.AnyAsync(user => user.Email == email.Trim().ToLowerInvariant(), cancellationToken);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }

        public async Task<IReadOnlyCollection<User>> GetClaimableUsersAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .Where(user =>
                    user.IsActive &&
                    user.Email == null &&
                    user.PasswordHash == null &&
                    user.MergeTargetUserId == null)
                .OrderBy(user => user.FirstName)
                .ThenBy(user => user.LastName)
                .ToListAsync(cancellationToken);
        }
    }
}
