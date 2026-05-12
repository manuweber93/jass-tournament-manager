using JassTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

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

        public DateTime? MergedAt { get; private set; }

        private User() { }

        public User(string? email, string? passwordHash, string firstName, string lastName, UserSourceType sourceType, bool isSysAdmin = DefaultIsSysAdmin)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name must not be empty.", nameof(firstName));
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name must not be empty.", nameof(lastName));
            }

            if (sourceType == UserSourceType.SelfRegistered)
            {
                if (email is null || string.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Email must not be null or empty for self registered users.", nameof(sourceType));
                }

                if (passwordHash is null || string.IsNullOrWhiteSpace(passwordHash))
                {
                    throw new ArgumentException("Password hash must not be null or empty for self registered users.", nameof(passwordHash));
                }
            }

            Email = email?.Trim().ToLowerInvariant();
            PasswordHash = passwordHash;
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

            if (mergeTargetUserId == Guid.Empty)
            {
                throw new ArgumentException("Target user id must not be empty.", nameof(mergeTargetUserId));
            }

            if (mergedByUserId == Guid.Empty)
            {
                throw new ArgumentException("Merged by user id must no be empty.", nameof(mergedByUserId));
            }

            MergeTargetUserId = mergeTargetUserId;
            MergedBy = mergedByUserId;
            MergedAt = DateTime.UtcNow;
        }
    }
}
