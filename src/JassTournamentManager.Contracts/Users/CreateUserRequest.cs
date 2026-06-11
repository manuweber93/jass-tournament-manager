using System.ComponentModel.DataAnnotations;

namespace JassTournamentManager.Contracts.Users
{
    public sealed record CreateUserRequest(
        [property: Required]
        [property: MaxLength(50)]
        string FirstName,

        [property: Required]
        [property: MaxLength(50)]
        string LastName);
}
