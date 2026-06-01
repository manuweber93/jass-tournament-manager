using JassTournamentManager.Application.Common;

namespace JassTournamentManager.Application.Users
{
    public static class UserErrors
    {
        public static readonly Error NotFound = new("Users.NotFound", "User does not exist.", ErrorType.NotFound);
    }
}
