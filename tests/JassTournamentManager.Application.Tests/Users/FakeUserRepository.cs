using JassTournamentManager.Application.Users;
using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Tests.Users
{
    internal sealed class FakeUserRepository : IUserRepository
    {
        private readonly List<User> _users = [];

        public IReadOnlyCollection<User> Users => _users.AsReadOnly();

        public Task AddAsync(User user, CancellationToken cancellationToken)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_users.Any(user => user.Id == userId));
        }
    }
}
