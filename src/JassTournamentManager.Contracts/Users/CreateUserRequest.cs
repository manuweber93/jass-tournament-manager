using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Users
{
    public sealed record CreateUserRequest(
        [Required]
        [MaxLength(50)]
        string FirstName,

        [Required]
        [MaxLength(50)]
        string LastName);
}

