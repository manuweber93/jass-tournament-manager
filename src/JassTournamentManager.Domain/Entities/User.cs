using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.Entities
{
    public class User : Common.BaseEntity
    {
        private const bool DefaultIsSysAdmin = false;

        public string? Email { get; private set; }

        public string? PasswordHash { get; private set; }

        public string FirstName { get; private set; } = string.Empty;

        public string LastName { get; private set; } = string.Empty;

        public bool IsSysAdmin { get; private set; }

        public UserSourceType SourceType { get; private set; }

        public Guid? MergeTargetUserId { get; private set; }

        public Guid? MergedBy {  get; private set; }

        public DateTimeOffset? MergedAt { get; private set; }

        private User() { }

        public User(string? email, string? passwordHash, string firstName, string lastName, UserSourceType sourceType, bool isSysAdmin = DefaultIsSysAdmin)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

            if (sourceType == UserSourceType.SelfRegistered)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(email);
                ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
            }

            Email = email?.Trim().ToLowerInvariant();
            PasswordHash = passwordHash?.Trim();
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            SourceType = sourceType;
            IsSysAdmin = isSysAdmin;
        }

        public void MergeIntoDifferentUser(Guid mergeTargetUserId, Guid mergedByUserId)
        {
            if (MergeTargetUserId is not null)
            {
                throw new InvalidOperationException("User has already been merged into different user.");
            }
            
            Guard.AgainstEmptyGuid(mergeTargetUserId, nameof(mergeTargetUserId));
            Guard.AgainstEmptyGuid(mergedByUserId, nameof(mergedByUserId));

            MergeTargetUserId = mergeTargetUserId;
            MergedBy = mergedByUserId;
            MergedAt = DateTimeOffset.UtcNow;
        }
    }
}
