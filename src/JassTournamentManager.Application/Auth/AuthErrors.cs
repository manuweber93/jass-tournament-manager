using JassTournamentManager.Application.Common;

namespace JassTournamentManager.Application.Auth
{
    public static class AuthErrors
    {
        public static readonly Error UserClaimingDisabled = new("Auth.UserClaimingDisabled", "User claiming is disabled.", ErrorType.Forbidden);

        public static readonly Error EmailAlreadyInUse = new("Auth.EmailAlreadyInUse", "The provided email address is already in use.", ErrorType.Conflict);

        public static readonly Error Unauthorized = new("Auth.Unauthorized", "Authentication is required to access this resource.", ErrorType.Unauthorized);

        public static readonly Error InvalidEmailAddress = new("Auth.InvalidEmailAddress", "Email must be a valid email address.", ErrorType.Invalid);

        public static readonly Error PasswordRequirementsNotMet = new("Auth.PasswordRequirementsNotMet", "Password must be at least 8 characters long and contain at least one number and one special character.", ErrorType.Invalid);
    }
}
