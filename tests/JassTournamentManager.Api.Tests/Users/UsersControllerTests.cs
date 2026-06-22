using FluentAssertions;
using JassTournamentManager.Api.Controllers;
using JassTournamentManager.Application.Auth;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.Users;
using JassTournamentManager.Contracts.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JassTournamentManager.Api.Tests.Users
{
    public class UsersControllerTests
    {
        private readonly FakeUserService _service;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _service = new FakeUserService();
            _controller = new UsersController(_service);
        }

        [Fact]
        public async Task CreateAsync_WithSuccess_ReturnsCreatedAtAction()
        {
            var request = UsersControllerTestData.CreateCreateUserRequest();
            var response = UsersControllerTestData.CreateUserResponse();
            _service.CreateAsyncResult = Result<UserResponse>.Success(response);

            var result = await _controller.CreateAsync(request, CancellationToken.None);

            var createdAtResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtResult.Value.Should().Be(response);
            createdAtResult.ActionName.Should().Be("GetById");
            createdAtResult.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(response.Id);
            _service.ReceivedCreateAsyncRequest.Should().Be(request);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidInput_ReturnsBadRequest()
        {
            var request = UsersControllerTestData.CreateCreateUserRequest();
            _service.CreateAsyncResult = Result<UserResponse>.Failure(CommonErrors.InvalidInput);

            var result = await _controller.CreateAsync(request, CancellationToken.None);

            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(CommonErrors.InvalidInput);
        }

        [Fact]
        public async Task GetByIdAsync_WithSuccess_ReturnsOk()
        {
            var userId = UsersControllerTestData.CreateId();
            var response = UsersControllerTestData.CreateUserResponse();
            _service.GetByIdAsyncResult = Result<UserResponse>.Success(response);

            var result = await _controller.GetByIdAsync(userId, CancellationToken.None);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(response);
            _service.ReceivedGetByIdAsyncRequest.Should().Be(userId);
        }

        [Fact]
        public async Task GetByIdAsync_WithNotFound_ReturnsNotFound()
        {
            var userId = UsersControllerTestData.CreateId();
            _service.GetByIdAsyncResult = Result<UserResponse>.Failure(UserErrors.NotFound);

            var result = await _controller.GetByIdAsync(userId, CancellationToken.None);

            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be(UserErrors.NotFound);
        }

        [Fact]
        public async Task GetClaimableUsers_WithSuccess_ReturnsOk()
        {
            var response = UsersControllerTestData.CreateClaimableUserResponses();
            _service.GetClaimableUsersResult = Result<IEnumerable<ClaimableUserResponse>>.Success(response);

            var result = await _controller.GetClaimableUsers(CancellationToken.None);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(response);
        }

        [Fact]
        public async Task GetClaimableUsers_WithUserClaimingDisabled_ReturnsForbidden()
        {
            _service.GetClaimableUsersResult = Result<IEnumerable<ClaimableUserResponse>>.Failure(AuthErrors.UserClaimingDisabled);

            var result = await _controller.GetClaimableUsers(CancellationToken.None);

            var forbiddenResult = result.Result.Should().BeOfType<ObjectResult>().Subject;
            forbiddenResult.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            forbiddenResult.Value.Should().Be(AuthErrors.UserClaimingDisabled);
        }

        [Fact]
        public async Task ResetPassword_WithSuccess_ReturnsOK()
        {
            var userId = UsersControllerTestData.CreateId();
            var request = UsersControllerTestData.CreateResetPasswordRequest();
            var response = true;
            _service.ResetPasswordResult = Result<bool>.Success(response);

            var result = await _controller.ResetPassword(userId, request, CancellationToken.None);

            result.Should().BeOfType<OkResult>();
            _service.ReceivedResetPasswordUserId.Should().Be(userId);
            _service.ReceivedResetPasswordRequest.Should().Be(request);
        }

        [Fact]
        public async Task ResetPassword_WithNotFound_ReturnsNotFound()
        {
            var userId = UsersControllerTestData.CreateId();
            var request = UsersControllerTestData.CreateResetPasswordRequest();
            _service.ResetPasswordResult = Result<bool>.Failure(UserErrors.NotFound);

            var result = await _controller.ResetPassword(userId, request, CancellationToken.None);

            var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be(UserErrors.NotFound);
        }
    }
}

