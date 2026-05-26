using JassTournamentManager.Application.Common;

namespace JassTournamentManager.Application.TournamentConfigs
{
    public static class TournamentConfigErrors
    {
        public static readonly Error InvalidInput = new("TournamentConfigs.InvalidInput", "The provided input for the tournament config is not valid.");
    }
}
