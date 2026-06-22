using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Users
{
    public sealed record ResetPasswordRequest(
        [Required]
        [MinLength(8)]
        [MaxLength(200)]
        string NewPassword);
}

