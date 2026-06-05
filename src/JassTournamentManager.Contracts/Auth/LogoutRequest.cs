using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Auth
{
    public sealed record LogoutRequest(
        [property: Required]
        string RefreshToken);
}
