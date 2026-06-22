namespace JassTournamentManager.Application.Common
{
    public static class CommonErrors
    {
        public static readonly Error InvalidInput = new("Common.InvalidInput", "Input provided is not valid.", ErrorType.Invalid);

        public static readonly Error Forbidden = new("Common.Forbidden", "You do not have permission to perform this action.", ErrorType.Forbidden);
    }
}
