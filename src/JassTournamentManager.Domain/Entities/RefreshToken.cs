using JassTournamentManager.Domain.Common;

namespace JassTournamentManager.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; private set; }

        public string TokenHash { get; private set; } = null!;

        public DateTimeOffset ExpiresAtUtc { get; private set; }

        public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAtUtc;

        public DateTimeOffset? RevokedAtUtc { get; private set; }

        public bool IsRevoked => RevokedAtUtc is not null;

        public Guid? ReplacedByTokenId { get; private set; }

        public bool IsActive => !IsExpired && !IsRevoked;

        private RefreshToken() { }

        public RefreshToken(Guid userId, string tokenHash, DateTimeOffset expiresAtUtc)
        {
            Guard.AgainstEmptyGuid(userId, nameof(userId));
            ArgumentException.ThrowIfNullOrWhiteSpace(tokenHash);
            Guard.AgainstDefaultDateTimeOffset(expiresAtUtc, nameof(expiresAtUtc));

            UserId = userId;
            TokenHash = tokenHash;
            ExpiresAtUtc = expiresAtUtc;
        }

        public void Revoke()
        {
            RevokedAtUtc = DateTimeOffset.UtcNow;
            MarkAsUpdated();
        }

        public void Replace(Guid newRefreshTokenId)
        {
            Guard.AgainstEmptyGuid(newRefreshTokenId, nameof(newRefreshTokenId));

            if (ReplacedByTokenId is not null)
            {
                throw new InvalidOperationException("The token has already been replaced.");
            }

            ReplacedByTokenId = newRefreshTokenId;
            Revoke();
        }
    }
}
