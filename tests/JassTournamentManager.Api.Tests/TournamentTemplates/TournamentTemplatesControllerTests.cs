using FluentAssertions;
using JassTournamentManager.Api.Controllers;
using JassTournamentManager.Application.Auth;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.TournamentTemplates;
using JassTournamentManager.Contracts.TournamentTemplates;
using Microsoft.AspNetCore.Mvc;

namespace JassTournamentManager.Api.Tests.TournamentTemplates
{
    public class TournamentTemplatesControllerTests
    {
        private readonly FakeTournamentTemplateService _service;
        private readonly TournamentTemplatesController _controller;

        public TournamentTemplatesControllerTests()
        {
            _service = new FakeTournamentTemplateService();
            _controller = new TournamentTemplatesController(_service);
        }

        [Fact]
        public async Task CreateAsync_WithSuccess_ReturnsCreatedAtAction()
        {
            var request = TournamentTemplatesControllerTestData.CreateCreateTournamentTemplateRequest();
            var response = TournamentTemplatesControllerTestData.CreateTournamentTemplateResponse();
            _service.CreateAsyncResult = Result<TournamentTemplateResponse>.Success(response);

            ActionResult<TournamentTemplateResponse> result = await _controller.CreateAsync(request, CancellationToken.None);

            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(TournamentTemplatesController.GetByIdAsync));
            createdResult.Value.Should().Be(response);
            createdResult.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(response.Id);
            _service.ReceivedCreateAsyncRequest.Should().Be(request);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidInput_ReturnsBadRequest()
        {
            var request = TournamentTemplatesControllerTestData.CreateCreateTournamentTemplateRequest();
            _service.CreateAsyncResult = Result<TournamentTemplateResponse>.Failure(CommonErrors.InvalidInput);

            ActionResult<TournamentTemplateResponse> result = await _controller.CreateAsync(request, CancellationToken.None);

            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(CommonErrors.InvalidInput);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidConfig_ReturnsBadRequest()
        {
            var request = TournamentTemplatesControllerTestData.CreateCreateTournamentTemplateRequest();
            _service.CreateAsyncResult = Result<TournamentTemplateResponse>.Failure(CommonErrors.InvalidInput);

            ActionResult<TournamentTemplateResponse> result = await _controller.CreateAsync(request, CancellationToken.None);

            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(CommonErrors.InvalidInput);
        }

        [Fact]
        public async Task CreateAsync_WithOrganizerNotFound_ReturnsNotFound()
        {
            var request = TournamentTemplatesControllerTestData.CreateCreateTournamentTemplateRequest();
            _service.CreateAsyncResult = Result<TournamentTemplateResponse>.Failure(TournamentTemplateErrors.OrganizerNotFound);

            ActionResult<TournamentTemplateResponse> result = await _controller.CreateAsync(request, CancellationToken.None);

            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be(TournamentTemplateErrors.OrganizerNotFound);
        }

        [Fact]
        public async Task CreateAsync_WithAlreadyExists_ReturnsConflict()
        {
            var request = TournamentTemplatesControllerTestData.CreateCreateTournamentTemplateRequest();
            _service.CreateAsyncResult = Result<TournamentTemplateResponse>.Failure(TournamentTemplateErrors.AlreadyExists);

            ActionResult<TournamentTemplateResponse> result = await _controller.CreateAsync(request, CancellationToken.None);

            var conflictResult = result.Result.Should().BeOfType<ConflictObjectResult>().Subject;
            conflictResult.Value.Should().Be(TournamentTemplateErrors.AlreadyExists);
        }

        [Fact]
        public async Task GetByIdAsync_WithSuccess_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var response = TournamentTemplatesControllerTestData.CreateTournamentTemplateResponse(id);
            _service.GetByIdAsyncResult = Result<TournamentTemplateResponse>.Success(response);

            ActionResult<TournamentTemplateResponse> result = await _controller.GetByIdAsync(id, CancellationToken.None);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(response);
            _service.ReceivedGetByIdAsyncRequest.Should().Be(id);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidInput_ReturnsBadRequest()
        {
            var id = Guid.NewGuid();
            _service.GetByIdAsyncResult = Result<TournamentTemplateResponse>.Failure(CommonErrors.InvalidInput);

            ActionResult<TournamentTemplateResponse> result = await _controller.GetByIdAsync(id, CancellationToken.None);

            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(CommonErrors.InvalidInput);
        }

        [Fact]
        public async Task GetByIdAsync_WithNotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            _service.GetByIdAsyncResult = Result<TournamentTemplateResponse>.Failure(TournamentTemplateErrors.NotFound);

            ActionResult<TournamentTemplateResponse> result = await _controller.GetByIdAsync(id, CancellationToken.None);

            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be(TournamentTemplateErrors.NotFound);
        }

        [Fact]
        public async Task GetForCurrentUserAsync_WithSuccess_ReturnsOk()
        {
            var response = TournamentTemplatesControllerTestData.CreateTournamentTemplateResponse();
            _service.GetForCurrentUserAsyncResult = Result<TournamentTemplateResponse>.Success(response);

            ActionResult<TournamentTemplateResponse> result = await _controller.GetForCurrentUserAsync(CancellationToken.None);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(response);
        }

        [Fact]
        public async Task GetForCurrentUserAsync_WithUnauthorized_ReturnsUnauthorized()
        {
            _service.GetForCurrentUserAsyncResult = Result<TournamentTemplateResponse>.Failure(AuthErrors.Unauthorized);

            ActionResult<TournamentTemplateResponse> result = await _controller.GetForCurrentUserAsync(CancellationToken.None);

            var unauthorizedResult = result.Result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
            unauthorizedResult.Value.Should().Be(AuthErrors.Unauthorized);
        }
    }
}
