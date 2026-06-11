namespace JassTournamentManager.Application.Auth
{
    public sealed record RefreshTokenSecret(
        string Token,
        string TokenHash,
        DateTimeOffset ExpiresAt);
}
