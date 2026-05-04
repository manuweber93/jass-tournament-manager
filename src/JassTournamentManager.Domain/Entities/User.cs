using JassTournamentManager.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JassTournamentManager.Domain.Entities
{
    public class User : Common.BaseEntity
    {

        public String Email { get; private set; } = String.Empty;

        public String PasswordHash { get; private set; } = String.Empty;

        public String FirstName { get; private set; } = String.Empty;

        public String LastName { get; private set;} = String.Empty;

        public UserRole Role { get; private set; }

        private User() { }

        public User(string email, string passwordHash, string firstName, string lastName, UserRole role)
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
            Role = role;
        }

    }
}
