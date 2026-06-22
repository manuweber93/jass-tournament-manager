using JassTournamentManager.Api.Common;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.TournamentTemplates;
using JassTournamentManager.Contracts.TournamentTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JassTournamentManager.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tournament-templates")]
    public sealed class TournamentTemplatesController : ControllerBase
    {
        private readonly ITournamentTemplateService _tournamentTemplateService;

        public TournamentTemplatesController(ITournamentTemplateService tournamentTemplateService)
        {
            _tournamentTemplateService = tournamentTemplateService ?? throw new ArgumentNullException(nameof(tournamentTemplateService));
        }

        [HttpPost]
        public async Task<ActionResult<TournamentTemplateResponse>> CreateAsync(CreateTournamentTemplateRequest request, CancellationToken cancellationToken)
        {
            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.CreateAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return CreatedAtAction("GetById", new { id = result.Value.Id }, result.Value);
            }

            return this.ToActionResult(result.Error);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TournamentTemplateResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.GetByIdAsync(id, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return this.ToActionResult(result.Error);
        }

        [HttpGet("me")]
        public async Task<ActionResult<TournamentTemplateResponse>> GetForCurrentUserAsync(CancellationToken cancellationToken)
        {
            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.GetForCurrentUserAsync(cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return this.ToActionResult(result.Error);
        }
    }
}

