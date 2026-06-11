using JassTournamentManager.Contracts.Users;

namespace JassTournamentManager.Contracts.Auth
{
    public sealed record AuthResponse(
        string AccessToken,
        DateTimeOffset AccessTokenExpiresAt,
        string RefreshToken,
        DateTimeOffset RefreshTokenExpiresAt,
        CurrentUserResponse User);
}
