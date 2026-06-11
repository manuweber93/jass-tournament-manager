namespace JassTournamentManager.Application.Auth
{
    public sealed record AccessTokenSecret(
        string Token,
        DateTimeOffset ExpiresAt);
}
