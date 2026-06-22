using JassTournamentManager.Application.Common;

namespace JassTournamentManager.Application.Auth
{
    public static class AuthErrors
    {
        public static readonly Error UserClaimingDisabled = new("Auth.UserClaimingDisabled", "User claiming is disabled.", ErrorType.Forbidden);

        public static readonly Error EmailAlreadyInUse = new("Auth.EmailAlreadyInUse", "The provided email address is already in use.", ErrorType.Conflict);

        public static readonly Error Unauthorized = new("Auth.Unauthorized", "Authentication is required to access this resource.", ErrorType.Unauthorized);
    }
}
