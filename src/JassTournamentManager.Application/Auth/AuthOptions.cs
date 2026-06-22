namespace JassTournamentManager.Application.Auth
{
    public sealed class AuthOptions
    {
        public const string SectionName = "Auth";

        public bool EnableUserClaiming { get; set; } = false;
    }
}
