using JassTournamentManager.Api.Common;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.Users;
using JassTournamentManager.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JassTournamentManager.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public sealed class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            Result<UserResponse> result = await _userService.CreateClaimableAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return CreatedAtAction("GetById", new { id = result.Value.Id }, result.Value);
            }

            return this.ToActionResult(result.Error);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Result<UserResponse> result = await _userService.GetByIdAsync(id, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return this.ToActionResult(result.Error);
        }

        [HttpGet("claimable")]
        public async Task<ActionResult<IEnumerable<ClaimableUserResponse>>> GetClaimableUsers(CancellationToken cancellationToken)
        {
            Result<IEnumerable<ClaimableUserResponse>> result = await _userService.GetClaimableUsers(cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return this.ToActionResult(result.Error);
        }

        [Authorize(Policy = "RequireSysAdmin")]
        [HttpPost("{id:guid}/password-reset")]
        public async Task<IActionResult> ResetPassword(Guid id, ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.ResetPassword(id, request, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return this.ToActionResult(result.Error);
        }
    }
}
