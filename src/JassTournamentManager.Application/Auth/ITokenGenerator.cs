using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Auth
{
    public interface ITokenGenerator
    {
        AccessTokenSecret GenerateAccessTokenSecret(User user);

        RefreshTokenSecret GenerateRefreshTokenSecret();

        string HashRefreshToken(string refreshToken);
    }
}
