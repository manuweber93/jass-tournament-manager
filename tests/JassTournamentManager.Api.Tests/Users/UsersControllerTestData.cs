using JassTournamentManager.Contracts.Users;

namespace JassTournamentManager.Api.Tests.Users
{
    internal static class UsersControllerTestData
    {
        public static Guid CreateId() => Guid.NewGuid();

        public static string CreateEmail() => "test.user@mail.com";

        public static string CreatePassword() => "very-secure";

        public static string CreateFirstName() => "Test";

        public static string CreateLastName() => "User";

        public static bool CreateIsSysAdmin() => false;

        public static UserResponse CreateUserResponse() => new(
            CreateId(),
            CreateEmail(),
            CreateFirstName(),
            CreateLastName(),
            CreateIsSysAdmin());

        public static CreateUserRequest CreateCreateUserRequest() => new(
            CreateFirstName(),
            CreateLastName());

        private static ClaimableUserResponse CreateClaimableUserResponse() => new(
            CreateId(),
            CreateFirstName(),
            CreateLastName());

        public static IEnumerable<ClaimableUserResponse> CreateClaimableUserResponses() => [
            CreateClaimableUserResponse(),
            CreateClaimableUserResponse(),
            CreateClaimableUserResponse()];

        public static ResetPasswordRequest CreateResetPasswordRequest() => new(
            CreatePassword());
    }
}
