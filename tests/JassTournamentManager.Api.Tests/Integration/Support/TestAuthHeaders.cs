namespace JassTournamentManager.Api.Tests.Integration.Support
{
    internal static class TestAuthHeaders
    {
        public const string UserIdHeaderName = "X-Test-UserId";

        public const string IsSysAdminHeaderName = "X-Test-IsSysAdmin";

        public static void AddAuthenticatedUser(this HttpRequestMessage request, Guid? userId = null, bool isSysAdmin = false)
        {
            request.Headers.Add(UserIdHeaderName, (userId ?? Guid.NewGuid()).ToString());
            request.Headers.Add(IsSysAdminHeaderName, isSysAdmin ? "true" : "false");
        }
    }
}


