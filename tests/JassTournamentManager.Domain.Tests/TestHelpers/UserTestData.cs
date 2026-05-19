using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class UserTestData
    {
        public static Guid CreateUserId() => Guid.NewGuid();

        public static string CreateEmail() => "player@tournament.com";

        public static string CreatePasswordHash() => "lkasjdfoewrjnfdskjsf";

        public static string CreateFirstName() => "Max";

        public static string CreateLastName() => "Muster";

        public static UserSourceType CreateSourceType() => UserSourceType.SelfRegistered;

        public static User CreateUser() => new(
            CreateEmail(),
            CreatePasswordHash(),
            CreateFirstName(),
            CreateLastName(),
            CreateSourceType());
    }
}
