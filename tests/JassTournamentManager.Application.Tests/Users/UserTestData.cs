using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Application.Tests.Users
{
    internal static class UserTestData
    {
        public static string CreateEmail() => "test.user@email.ch";

        public static string CreatePasswordHash() => "abc";

        public static string CreateFirstName() => "Test";

        public static string CreateLastName() => "User";

        public static User CreateUser() => new(
            CreateEmail(),
            CreatePasswordHash(),
            CreateFirstName(),
            CreateLastName(),
            UserSourceType.Manual);
    }
}
