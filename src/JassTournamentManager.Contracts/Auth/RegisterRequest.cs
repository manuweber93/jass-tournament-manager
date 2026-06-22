using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Auth
{
    public sealed record RegisterRequest(
        Guid? ClaimedUserId,

        [Required]
        [MaxLength(320)]
        [EmailAddress]
        string Email,

        [Required]
        [MinLength(8)]
        [MaxLength(200)]
        string Password,

        [Required]
        [MaxLength(50)]
        string FirstName,

        [Required]
        [MaxLength(50)]
        string LastName);
}

