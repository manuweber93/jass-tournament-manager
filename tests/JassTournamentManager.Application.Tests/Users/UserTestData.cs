using JassTournamentManager.Contracts.Users;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Application.Tests.Users
{
    internal static class UserTestData
    {
        public static Guid CreateUserId() => Guid.NewGuid();

        public static string CreateEmail() => "test.user@email.ch";

        public static string CreatePassword() => "abc";

        public static string CreatePasswordHash() => "abc-hash";

        public static string CreateFirstName() => "Test";

        public static string CreateLastName() => "User";

        public static User CreateUser() => new(
            CreateEmail(),
            CreatePasswordHash(),
            CreateFirstName(),
            CreateLastName(),
            UserSourceType.Manual);

        public static User CreateClaimableUser() => new(
            null,
            null,
            CreateFirstName(),
            CreateLastName(),
            UserSourceType.Manual);

        public static CreateUserRequest CreateCreateUserRequest() => new(
            CreateFirstName(),
            CreateLastName());
    }
}
