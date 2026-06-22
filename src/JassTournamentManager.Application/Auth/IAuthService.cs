using JassTournamentManager.Application.Common;
using JassTournamentManager.Contracts.Auth;

namespace JassTournamentManager.Application.Auth
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

        Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken);

        Task<Result<AuthResponse>> RefreshSessionAsync(RefreshSessionRequest request, CancellationToken cancellationToken);

        Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    }
}