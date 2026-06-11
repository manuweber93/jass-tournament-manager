using JassTournamentManager.Application.Common;

namespace JassTournamentManager.Application.Users
{
    public static class UserErrors
    {
        public static readonly Error NotFound = new("Users.NotFound", "User does not exist.", ErrorType.NotFound);

        public static readonly Error AlreadyExists = new("Users.AlreadyExists", "User with that email adress already exists.", ErrorType.Conflict);
    }
}
