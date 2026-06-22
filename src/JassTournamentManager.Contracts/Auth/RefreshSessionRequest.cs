using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Auth
{
    public sealed record RefreshSessionRequest(
        [Required]
        string RefreshToken);
}

