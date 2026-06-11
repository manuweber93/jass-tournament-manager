namespace JassTournamentManager.Contracts.Users
{
    public sealed record ClaimableUserResponse(
        Guid Id,
        string FirstName,
        string LastName);
}
