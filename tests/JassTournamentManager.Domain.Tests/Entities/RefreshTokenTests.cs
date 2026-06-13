using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Tests.TestHelpers;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class RefreshTokenTests
    {
        [Fact]
        public void Constructor_WithEmptyUserId_ThrowsArgumentException()
        {
            var emptyGuid = Guid.Empty;

            Action act = () => new RefreshToken(
                emptyGuid,
                RefreshTokenTestData.CreateTokenHash(),
                RefreshTokenTestData.CreateExpiresAtUtc()
                );

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithEmptyTokenHash_ThrowsArgumentException()
        {
            var emptyTokenHash = " ";

            Action act = () => new RefreshToken(
                UserTestData.CreateUserId(),
                emptyTokenHash,
                RefreshTokenTestData.CreateExpiresAtUtc());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithDefaultDateTimeOffset_ThrowsArgumentException()
        {
            DateTimeOffset defaultDateTimeOffset = default;

            Action act = () => new RefreshToken(
                UserTestData.CreateUserId(),
                RefreshTokenTestData.CreateTokenHash(),
                defaultDateTimeOffset);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesRefreshToken()
        {
            var userId = UserTestData.CreateUserId();
            var tokenHash = RefreshTokenTestData.CreateTokenHash();
            var expiresAt = RefreshTokenTestData.CreateExpiresAtUtc();

            var refreshToken = new RefreshToken(
                userId,
                tokenHash,
                expiresAt);

            refreshToken.UserId.Should().Be(userId);
            refreshToken.TokenHash.Should().Be(tokenHash);
            refreshToken.ExpiresAtUtc.Should().Be(expiresAt);
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesActiveRefreshToken()
        {
            var refreshToken = RefreshTokenTestData.CreateRefreshToken();

            refreshToken.IsActive.Should().BeTrue();
        }

        [Fact]
        public void Revoke_SetsRevokedAtUtc()
        {
            var refreshToken = RefreshTokenTestData.CreateRefreshToken();

            refreshToken.Revoke();

            refreshToken.RevokedAtUtc.Should().NotBeNull();
        }

        [Fact]
        public void Revoke_DeactivatesRefreshToken()
        {
            var refreshToken = RefreshTokenTestData.CreateRefreshToken();

            refreshToken.Revoke();

            refreshToken.IsActive.Should().BeFalse();
        }

        [Fact]
        public void Replace_WithEmptyNewRefreshTokenId_ThrowsArgumentException()
        {
            var refreshToken = RefreshTokenTestData.CreateRefreshToken();
            var emptyGuid = Guid.Empty;

            Action act = () => refreshToken.Replace(emptyGuid);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Replace_ForAlreadyReplacedRefreshToken_ThrowsInvalidOperationException()
        {
            var refreshToken = RefreshTokenTestData.CreateRefreshToken();
            refreshToken.Replace(RefreshTokenTestData.CreateRefreshTokenId());

            Action act = () => refreshToken.Replace(RefreshTokenTestData.CreateRefreshTokenId());

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Replace_WithValidRefreshTokenId_ReplacesAndRevokesRefreshToken()
        {
            var refreshToken = RefreshTokenTestData.CreateRefreshToken();
            var newRefreshTokenId = RefreshTokenTestData.CreateRefreshTokenId();

            refreshToken.Replace(newRefreshTokenId);

            refreshToken.ReplacedByTokenId.Should().Be(newRefreshTokenId);
            refreshToken.IsRevoked.Should().BeTrue();
            refreshToken.IsActive.Should().BeFalse();
        }
    }
}
