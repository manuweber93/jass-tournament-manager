using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Auth
{
    public sealed record RegisterRequest(
        Guid? ClaimedUserId,

        [property: Required]
        [property: MaxLength(320)]
        [property: EmailAddress]
        string Email,

        [property: Required]
        [property: MinLength(8)]
        [property: MaxLength(200)]
        string Password,

        [property: Required]
        [property: MaxLength(50)]
        string FirstName,

        [property: Required]
        [property: MaxLength(50)]
        string LastName);
}
