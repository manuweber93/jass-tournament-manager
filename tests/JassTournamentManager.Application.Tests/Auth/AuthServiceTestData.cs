using JassTournamentManager.Contracts.Auth;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Application.Tests.Auth
{
    internal static class AuthServiceTestData
    {
        public static Guid CreateClaimedUserId() => Guid.NewGuid();

        public static string CreateEmail() => "test.user@mail.com";

        public static string CreateExistingEmail() => "existing.user@mail.com";

        public static string CreateUsedEmail() => "used.user@mail.com";

        public static string CreateInvalidEmail() => "abc";

        public static string CreatePassword() => "very_s3cure";

        public static string CreateWeakPassword() => "abc";

        public static string CreateWrongPassword() => "wrong-password";

        public static string CreatePasswordHash() => "stored-password-hash";

        public static string CreateFirstName() => "Test";

        public static string CreateLastName() => "User";

        public static string CreateMissingRefreshToken() => "missing-refresh-token";

        public static DateTimeOffset CreateRefreshTokenExpiresAtUtc() =>
            new(2043, 11, 30, 2, 0, 0, TimeSpan.Zero);

        public static RegisterRequest CreateRegisterRequest(
            Guid? claimedUserId,
            string? email = null,
            string? password = null,
            string? firstName = null,
            string? lastName = null
            ) => new(
                claimedUserId,
                email ?? CreateEmail(),
                password ?? CreatePassword(),
                firstName ?? CreateFirstName(),
                lastName ?? CreateLastName());

        public static RegisterRequest CreateNewUserRegisterRequest() =>
            CreateRegisterRequest(claimedUserId: null);

        public static RegisterRequest CreateClaimExistingUserRequest(Guid claimedUserId) =>
            CreateRegisterRequest(claimedUserId);

        public static LoginRequest CreateLoginRequest(User user, string? password = null) =>
            new(user.Email!, password ?? CreatePassword());

        public static RefreshSessionRequest CreateRefreshSessionRequest(string refreshToken) =>
            new(refreshToken);

        public static LogoutRequest CreateLogoutRequest(string refreshToken) =>
            new(refreshToken);

        public static User CreateLoginUser(string? email = null, string? passwordHash = null) =>
            new(
                email ?? CreateEmail(),
                passwordHash ?? CreatePasswordHash(),
                CreateFirstName(),
                CreateLastName(),
                UserSourceType.SelfRegistered);

        public static User CreateClaimableUser() =>
            new(
                email: null,
                passwordHash: null,
                "Imported",
                "Player",
                UserSourceType.Imported);

        public static RefreshToken CreateActiveRefreshToken(Guid userId, string tokenHash) =>
            new(
                userId,
                tokenHash,
                CreateRefreshTokenExpiresAtUtc());

        public static RefreshToken CreateRevokedRefreshToken(Guid userId, string tokenHash)
        {
            var refreshToken = CreateActiveRefreshToken(userId, tokenHash);
            refreshToken.Revoke();
            return refreshToken;
        }
    }
}
