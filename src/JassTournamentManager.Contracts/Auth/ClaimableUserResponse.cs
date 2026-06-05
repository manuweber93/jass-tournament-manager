namespace JassTournamentManager.Contracts.Auth
{
    public sealed record ClaimableUserResponse(
        Guid Id,
        string FirstName,
        string LastName);
}
