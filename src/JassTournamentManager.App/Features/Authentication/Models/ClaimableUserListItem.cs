namespace JassTournamentManager.App.Features.Authentication.Models
{
    public sealed record ClaimableUserListItem(
    Guid Id,
    string FirstName,
    string LastName)
    {
        public string DisplayName => $"{FirstName} {LastName}";
    }
}
