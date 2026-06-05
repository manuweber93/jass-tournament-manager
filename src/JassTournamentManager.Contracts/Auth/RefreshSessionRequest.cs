using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Auth
{
    public sealed record RefreshSessionRequest(
        [property: Required]
        string RefreshToken);
}
