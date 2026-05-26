using JassTournamentManager.Application.Common;

namespace JassTournamentManager.Application.TournamentTemplates
{
    public static class TournamentTemplateErrors
    {
        public static readonly Error OrganizerNotFound = new("TournamentTemplates.OrganizerNotFound", "Organizer not found.");

        public static readonly Error AlreadyExists = new("TournamentTemplates.AlreadyExists", "Organizer already has a tournament template.");

        public static readonly Error InvalidInput = new("TournamentTemplates.InvalidInput", "Input provided for the new tournament template is not valid.");

        public static readonly Error NotFound = new("TournamentTemplates.NotFound", "No tournament template with the given id found.");
    }
}
