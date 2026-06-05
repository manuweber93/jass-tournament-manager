namespace JassTournamentManager.Contracts.Auth
{
    public sealed record CurrentUserResponse(
        Guid Id,
        string Email,
        string FirstName,
        string LastName,
        bool IsSysAdmin);
}
