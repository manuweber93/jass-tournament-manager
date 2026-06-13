using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Infrastructure.Auth;
using Microsoft.Extensions.Options;

namespace JassTournamentManager.Infrastructure.Tests.Auth
{
    internal static class TokenGeneratorTestData
    {
        public static TokenGenerator CreateTokenGenerator(JwtOptions jwtOptions)
        {
            return new TokenGenerator(Options.Create(jwtOptions));
        }

        public static JwtOptions CreateJwtOptions()
        {
            return new JwtOptions
            {
                Issuer = "jass-tournament-manager-tests",
                Audience = "jass-tournament-manager-test-api",
                Secret = "test-secret-with-at-least-32-characters",
                AccessTokenMinutes = 15,
                RefreshTokenDays = 30,
            };
        }

        public static User CreateUser(bool isSysAdmin)
        {
            return new User(
                "token.user@example.com",
                "password-hash",
                "Token",
                "User",
                UserSourceType.SelfRegistered,
                isSysAdmin: isSysAdmin);
        }
    }
}
