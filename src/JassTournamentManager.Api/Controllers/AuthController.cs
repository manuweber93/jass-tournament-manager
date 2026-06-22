using JassTournamentManager.Api.Common;
using JassTournamentManager.Application.Auth;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JassTournamentManager.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            Result<AuthResponse> result = await _authService.RegisterAsync(request, cancellationToken);
            return ToAuthResponse(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            Result<AuthResponse> result = await _authService.LoginAsync(request, cancellationToken);
            return ToAuthResponse(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> RefreshSessionAsync(RefreshSessionRequest request, CancellationToken cancellationToken)
        {
            Result<AuthResponse> result = await _authService.RefreshSessionAsync(request, cancellationToken);
            return ToAuthResponse(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync(LogoutRequest request, CancellationToken cancellationToken)
        {
            await _authService.LogoutAsync(request, cancellationToken);
            return Ok();
        }

        private ActionResult<AuthResponse> ToAuthResponse(Result<AuthResponse> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return this.ToActionResult(result.Error);
        }
    }
}
