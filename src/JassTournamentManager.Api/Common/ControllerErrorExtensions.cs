using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.TournamentConfigs;
using JassTournamentManager.Application.TournamentTemplates;
using Microsoft.AspNetCore.Mvc;

namespace JassTournamentManager.Api.Common
{
    internal static class ControllerErrorExtensions
    {
        public static ActionResult ToActionResult(this ControllerBase controller, Error error)
        {
            if (error == TournamentTemplateErrors.InvalidInput ||
                error == TournamentConfigErrors.InvalidInput)
            {
                return controller.BadRequest(error);
            }

            if (error == TournamentTemplateErrors.OrganizerNotFound ||
                error == TournamentTemplateErrors.NotFound)
            {
                return controller.NotFound(error);
            }

            if (error == TournamentTemplateErrors.AlreadyExists)
            {
                return controller.Conflict(error);
            }

            return controller.Problem(
                title: "Unexpected application error.",
                detail: error.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
