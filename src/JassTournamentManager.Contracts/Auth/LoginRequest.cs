using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Auth
{
    public sealed record LoginRequest(
        [Required]
        [EmailAddress]
        [MaxLength(320)]
        string Email,

        [Required]
        string Password);
}

