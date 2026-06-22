using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Users
{
    public sealed record ResetPasswordRequest(
        [property: Required]
        [property: MinLength(8)]
        [property: MaxLength(200)]
        string NewPassword);
}
