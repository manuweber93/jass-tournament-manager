using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Auth
{
    public sealed record LogoutRequest(
        [Required]
        string RefreshToken);
}

