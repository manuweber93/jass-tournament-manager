using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JassTournamentManager.Infrastructure.Tests.Auth
{
    public class TokenGeneratorTests
    {
        [Fact]
        public void GenerateAccessTokenSecret_WithUser_ReturnsJwtWithConfiguredIssuerAudienceAndClaims()
        {
            var jwtOptions = TokenGeneratorTestData.CreateJwtOptions();
            var tokenGenerator = TokenGeneratorTestData.CreateTokenGenerator(jwtOptions);
            var user = TokenGeneratorTestData.CreateUser(isSysAdmin: true);

            var accessTokenSecret = tokenGenerator.GenerateAccessTokenSecret(user);

            accessTokenSecret.Token.Should().NotBeNullOrWhiteSpace();
            accessTokenSecret.ExpiresAt.Should().BeCloseTo(
                DateTimeOffset.UtcNow.AddMinutes(jwtOptions.AccessTokenMinutes),
                TimeSpan.FromSeconds(5));

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessTokenSecret.Token);
            jwt.Issuer.Should().Be(jwtOptions.Issuer);
            jwt.Audiences.Should().ContainSingle().Which.Should().Be(jwtOptions.Audience);
            jwt.Claims.Single(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value.Should().Be(user.Id.ToString());
            jwt.Claims.Single(claim => claim.Type == "is_sys_admin").Value.Should().Be("true");
            jwt.Claims.Should().Contain(claim => claim.Type == JwtRegisteredClaimNames.Jti);
            jwt.Claims.Should().Contain(claim =>
                claim.Type == JwtRegisteredClaimNames.Iat
                && claim.ValueType == ClaimValueTypes.Integer64);
        }

        [Fact]
        public void GenerateAccessTokenSecret_WithNonSysAdminUser_SetsSysAdminClaimToFalse()
        {
            var tokenGenerator = TokenGeneratorTestData.CreateTokenGenerator(TokenGeneratorTestData.CreateJwtOptions());
            var user = TokenGeneratorTestData.CreateUser(isSysAdmin: false);

            var accessTokenSecret = tokenGenerator.GenerateAccessTokenSecret(user);

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(accessTokenSecret.Token);
            jwt.Claims.Single(claim => claim.Type == "is_sys_admin").Value.Should().Be("false");
        }

        [Fact]
        public void GenerateRefreshTokenSecret_ReturnsTokenHashAndExpiration()
        {
            var jwtOptions = TokenGeneratorTestData.CreateJwtOptions();
            var tokenGenerator = TokenGeneratorTestData.CreateTokenGenerator(jwtOptions);

            var refreshTokenSecret = tokenGenerator.GenerateRefreshTokenSecret();

            refreshTokenSecret.Token.Should().NotBeNullOrWhiteSpace();
            refreshTokenSecret.TokenHash.Should().Be(tokenGenerator.HashRefreshToken(refreshTokenSecret.Token));
            refreshTokenSecret.ExpiresAt.Should().BeCloseTo(
                DateTimeOffset.UtcNow.AddDays(jwtOptions.RefreshTokenDays),
                TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void GenerateRefreshTokenSecret_WhenCalledTwice_ReturnsDifferentTokens()
        {
            var tokenGenerator = TokenGeneratorTestData.CreateTokenGenerator(TokenGeneratorTestData.CreateJwtOptions());

            var firstRefreshToken = tokenGenerator.GenerateRefreshTokenSecret();
            var secondRefreshToken = tokenGenerator.GenerateRefreshTokenSecret();

            secondRefreshToken.Token.Should().NotBe(firstRefreshToken.Token);
            secondRefreshToken.TokenHash.Should().NotBe(firstRefreshToken.TokenHash);
        }

        [Fact]
        public void HashRefreshToken_WithToken_ReturnsSha256HexHash()
        {
            var tokenGenerator = TokenGeneratorTestData.CreateTokenGenerator(TokenGeneratorTestData.CreateJwtOptions());
            var refreshToken = "refresh-token";
            var expectedHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));

            var tokenHash = tokenGenerator.HashRefreshToken(refreshToken);

            tokenHash.Should().Be(expectedHash);
        }
    }
}
