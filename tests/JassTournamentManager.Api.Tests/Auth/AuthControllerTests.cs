using FluentAssertions;
using JassTournamentManager.Api.Controllers;
using JassTournamentManager.Application.Auth;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Contracts.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JassTournamentManager.Api.Tests.Auth
{
    public class AuthControllerTests
    {
        private readonly FakeAuthService _service;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _service = new FakeAuthService();
            _controller = new AuthController(_service);
        }

        [Fact]
        public async Task RegisterAsync_WithSuccess_ReturnsOk()
        {
            var request = AuthControllerTestData.CreateRegisterRequest();
            var response = AuthControllerTestData.CreateAuthResponse();
            _service.RegisterAsyncResult = Result<AuthResponse>.Success(response);

            ActionResult<AuthResponse> result = await _controller.RegisterAsync(request, CancellationToken.None);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(response);
            _service.ReceivedRegisterAsyncRequest.Should().Be(request);
        }

        [Fact]
        public async Task RegisterAsync_WithInvalidInput_ReturnsBadRequest()
        {
            var request = AuthControllerTestData.CreateRegisterRequest();
            _service.RegisterAsyncResult = Result<AuthResponse>.Failure(CommonErrors.InvalidInput);

            ActionResult<AuthResponse> result = await _controller.RegisterAsync(request, CancellationToken.None);

            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(CommonErrors.InvalidInput);
        }

        [Fact]
        public async Task RegisterAsync_WithUserClaimingDisabled_ReturnsForbidden()
        {
            var request = AuthControllerTestData.CreateRegisterRequestWithUserClaiming();
            _service.RegisterAsyncResult = Result<AuthResponse>.Failure(AuthErrors.UserClaimingDisabled);

            ActionResult<AuthResponse> result = await _controller.RegisterAsync(request, CancellationToken.None);

            var forbiddenResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            forbiddenResult.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            forbiddenResult.Value.Should().Be(AuthErrors.UserClaimingDisabled);
        }

        [Fact]
        public async Task LoginAsync_WithSuccess_ReturnsOk()
        {
            var request = AuthControllerTestData.CreateLoginRequest();
            var response = AuthControllerTestData.CreateAuthResponse();
            _service.LoginAsyncResult = Result<AuthResponse>.Success(response);

            ActionResult<AuthResponse> result = await _controller.LoginAsync(request, CancellationToken.None);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(response);
            _service.ReceivedLoginAsyncRequest.Should().Be(request);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidInput_ReturnsBadRequest()
        {
            var request = AuthControllerTestData.CreateLoginRequest();
            _service.LoginAsyncResult = Result<AuthResponse>.Failure(CommonErrors.InvalidInput);

            ActionResult<AuthResponse> result = await _controller.LoginAsync(request, CancellationToken.None);

            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(CommonErrors.InvalidInput);
        }

        [Fact]
        public async Task RefreshSessionAsync_WithSuccess_ReturnsOk()
        {
            var request = AuthControllerTestData.CreateRefreshSessionRequest();
            var response = AuthControllerTestData.CreateAuthResponse();
            _service.RefreshSessionAsyncResult = Result<AuthResponse>.Success(response);

            ActionResult<AuthResponse> result = await _controller.RefreshSessionAsync(request, CancellationToken.None);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(response);
            _service.ReceivedRefreshSessionAsyncRequest.Should().Be(request);
        }

        [Fact]
        public async Task RefreshSessionAsync_WithInvalidInput_ReturnsBadRequest()
        {
            var request = AuthControllerTestData.CreateRefreshSessionRequest();
            _service.RefreshSessionAsyncResult = Result<AuthResponse>.Failure(CommonErrors.InvalidInput);

            ActionResult<AuthResponse> result = await _controller.RefreshSessionAsync(request, CancellationToken.None);

            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(CommonErrors.InvalidInput);
        }

        [Fact]
        public async Task LogoutAsync_WithSuccess_ReturnsOk()
        {
            var request = AuthControllerTestData.CreateLogoutRequest();

            var result = await _controller.LogoutAsync(request, CancellationToken.None);

            result.Should().BeOfType<OkResult>();
            _service.ReceivedLogoutAsyncRequest.Should().Be(request);
        }
    }
}
