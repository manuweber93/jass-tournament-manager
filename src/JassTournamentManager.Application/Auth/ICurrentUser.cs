namespace JassTournamentManager.Application.Auth
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        Guid UserId { get; }

        string? Email { get; }

        bool IsSysAdmin { get; }
    }
}
