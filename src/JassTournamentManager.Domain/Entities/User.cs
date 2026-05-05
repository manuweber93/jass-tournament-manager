using JassTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JassTournamentManager.Domain.Entities
{
    public class User : Common.BaseEntity
    {

        public string Email { get; private set; } = string.Empty;

        public string PasswordHash { get; private set; } = string.Empty;

        public string FirstName { get; private set; } = string.Empty;

        public string LastName { get; private set; } = string.Empty;

        public bool IsSysAdmin { get; private set; } = false;

        private User() { }

        public User(string email, string passwordHash, string firstName, string lastName, bool isSysAdmin)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email must not be empty.", nameof(email));
            }

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Password hash must not be empty.", nameof(passwordHash));
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name must not be empty.", nameof(firstName));
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name must not be empty.", nameof(lastName));
            }

            Email = email.Trim().ToLowerInvariant();
            PasswordHash = passwordHash;
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            IsSysAdmin = isSysAdmin;
        }

    }
}
