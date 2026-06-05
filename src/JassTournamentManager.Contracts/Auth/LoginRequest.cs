using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Auth
{
    public sealed record LoginRequest(
        [property: Required]
        [property: EmailAddress]
        [property: MaxLength(320)]
        string Email,

        [property: Required]
        string Password);
}
