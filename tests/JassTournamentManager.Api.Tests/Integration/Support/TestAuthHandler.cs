using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace JassTournamentManager.Api.Tests.Integration.Support
{
    internal sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "Test";

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(TestAuthHeaders.UserIdHeaderName, out var userIdValues))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            string? userId = userIdValues.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Task.FromResult(AuthenticateResult.Fail("Test user id header is empty."));
            }

            bool isSysAdmin = Request.Headers.TryGetValue(TestAuthHeaders.IsSysAdminHeaderName, out var isSysAdminValues)
                && string.Equals(isSysAdminValues.FirstOrDefault(), "true", StringComparison.OrdinalIgnoreCase);

            Claim[] claims =
            [
                new(JwtRegisteredClaimNames.Sub, userId),
                new("is_sys_admin", isSysAdmin ? "true" : "false")
            ];

            var identity = new ClaimsIdentity(claims, SchemeName);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}


