using JassTournamentManager.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace JassTournamentManager.Api.Common
{
    internal static class ControllerErrorExtensions
    {
        public static ActionResult ToActionResult(this ControllerBase controller, Error error) =>
            error.Type switch
            {
                ErrorType.Invalid => controller.BadRequest(error),
                ErrorType.NotFound => controller.NotFound(error),
                ErrorType.Conflict => controller.Conflict(error),
                _ => controller.Problem(
                    title: "Unexpected application error.",
                    detail: error.Message,
                    statusCode: StatusCodes.Status500InternalServerError)
            };
    }
}
