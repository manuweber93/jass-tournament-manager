using JassTournamentManager.Application.Auth;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Contracts.Auth;

namespace JassTournamentManager.Api.Tests.Auth
{
    internal sealed class FakeAuthService : IAuthService
    {
        public Result<AuthResponse>? LoginAsyncResult { get; set; }

        public Result<AuthResponse>? RefreshSessionAsyncResult { get; set; }

        public Result<AuthResponse>? RegisterAsyncResult { get; set; }

        public LoginRequest? ReceivedLoginAsyncRequest { get; private set; }

        public LogoutRequest? ReceivedLogoutAsyncRequest { get; private set; }

        public RefreshSessionRequest? ReceivedRefreshSessionAsyncRequest { get; private set; }

        public RegisterRequest? ReceivedRegisterAsyncRequest { get; private set; }

        public int LogoutAsyncCallCount { get; private set; }

        public Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            ReceivedLoginAsyncRequest = request;
            return Task.FromResult(LoginAsyncResult!);
        }

        public Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken)
        {
            ReceivedLogoutAsyncRequest = request;
            LogoutAsyncCallCount++;
            return Task.CompletedTask;
        }

        public Task<Result<AuthResponse>> RefreshSessionAsync(RefreshSessionRequest request, CancellationToken cancellationToken)
        {
            ReceivedRefreshSessionAsyncRequest = request;
            return Task.FromResult(RefreshSessionAsyncResult!);
        }

        public Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            ReceivedRegisterAsyncRequest = request;
            return Task.FromResult(RegisterAsyncResult!);
        }
    }
}

