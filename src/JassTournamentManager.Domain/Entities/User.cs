using JassTournamentManager.Domain.Common;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.Entities
{
    public class User : BaseEntity
    {
        private const bool DefaultIsSysAdmin = false;
        private const bool DefaultIsActive = true;

        public string? Email { get; private set; }

        public string? PasswordHash { get; private set; }

        public string FirstName { get; private set; } = string.Empty;

        public string LastName { get; private set; } = string.Empty;

        public bool IsActive { get; private set; }

        public bool IsSysAdmin { get; private set; }

        public UserSourceType SourceType { get; private set; }

        public Guid? MergeTargetUserId { get; private set; }

        public Guid? MergedBy {  get; private set; }

        public DateTimeOffset? MergedAt { get; private set; }

        private User() { }

        public User(string? email, string? passwordHash, string firstName, string lastName, UserSourceType sourceType, bool isActive = DefaultIsActive, bool isSysAdmin = DefaultIsSysAdmin)
        {
            VerifyArguments(email, passwordHash, firstName, lastName, sourceType);

            Email = email?.Trim().ToLowerInvariant();
            PasswordHash = passwordHash?.Trim();
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            SourceType = sourceType;
            IsActive = isActive;
            IsSysAdmin = isSysAdmin;
        }

        public void Update(string email, string passwordHash, string firstName, string lastName, bool isActive, bool isSysAdmin)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

            Email = email.Trim().ToLowerInvariant();
            PasswordHash = passwordHash.Trim();
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            IsActive = isActive;
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

            if (mergeTargetUserId == Id)
            {
                throw new InvalidOperationException("User cannot be merged into itself.");
            }

            MergeTargetUserId = mergeTargetUserId;
            MergedBy = mergedByUserId;
            MergedAt = DateTimeOffset.UtcNow;
        }

        private static void VerifyArguments(string? email, string? passwordHash, string firstName, string lastName, UserSourceType sourceType)
        {
            Guard.AgainstOptionalMaxLength(email, 320, nameof(email));

            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            Guard.AgainstMaxLength(firstName, 50, nameof(firstName));

            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
            Guard.AgainstMaxLength(lastName, 50, nameof(lastName));

            if (sourceType == UserSourceType.SelfRegistered)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(email);
                ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
            }
        }
    }
}
