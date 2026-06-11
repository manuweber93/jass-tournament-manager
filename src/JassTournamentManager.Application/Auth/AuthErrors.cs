using JassTournamentManager.Application.Common;

namespace JassTournamentManager.Application.Auth
{
    public static class AuthErrors
    {
        public static readonly Error InvalidInput = new("Auth.InvalidInput", "Input provided for the authentication is not valid.", ErrorType.Invalid);

        public static readonly Error EmailAlreadyInUse = new("Auth.EmailAlreadyInUse", "The provided email address is already in use.", ErrorType.Conflict);
    }
}
