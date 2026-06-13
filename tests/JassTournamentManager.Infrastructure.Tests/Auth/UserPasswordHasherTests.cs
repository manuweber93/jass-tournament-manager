using FluentAssertions;
using JassTournamentManager.Infrastructure.Auth;

namespace JassTournamentManager.Infrastructure.Tests.Auth
{
    public class UserPasswordHasherTests
    {
        [Fact]
        public void HashPassword_WithPassword_ReturnsNonPlaintextHash()
        {
            var passwordHasher = new UserPasswordHasher();
            var password = "correct-password";

            var passwordHash = passwordHasher.HashPassword(password);

            passwordHash.Should().NotBeNullOrWhiteSpace();
            passwordHash.Should().NotBe(password);
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
        {
            var passwordHasher = new UserPasswordHasher();
            var password = "correct-password";
            var passwordHash = passwordHasher.HashPassword(password);

            var isValid = passwordHasher.VerifyPassword(passwordHash, password);

            isValid.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_WithWrongPassword_ReturnsFalse()
        {
            var passwordHasher = new UserPasswordHasher();
            var passwordHash = passwordHasher.HashPassword("correct-password");

            var isValid = passwordHasher.VerifyPassword(passwordHash, "wrong-password");

            isValid.Should().BeFalse();
        }
    }
}
