using FluentAssertions;
using JassTournamentManager.Api.Controllers;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.TournamentConfigs;
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
            var request = TournamentTemplateControllerTestData.CreateCreateTournamentTemplateRequest();
            var response = TournamentTemplateControllerTestData.CreateTournamentTemplateResponse();
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
            var request = TournamentTemplateControllerTestData.CreateCreateTournamentTemplateRequest();
            _service.CreateAsyncResult = Result<TournamentTemplateResponse>.Failure(TournamentTemplateErrors.InvalidInput);

            ActionResult<TournamentTemplateResponse> result = await _controller.CreateAsync(request, CancellationToken.None);

            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(TournamentTemplateErrors.InvalidInput);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidConfig_ReturnsBadRequest()
        {
            var request = TournamentTemplateControllerTestData.CreateCreateTournamentTemplateRequest();
            _service.CreateAsyncResult = Result<TournamentTemplateResponse>.Failure(TournamentConfigErrors.InvalidInput);

            ActionResult<TournamentTemplateResponse> result = await _controller.CreateAsync(request, CancellationToken.None);

            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(TournamentConfigErrors.InvalidInput);
        }

        [Fact]
        public async Task CreateAsync_WithOrganizerNotFound_ReturnsNotFound()
        {
            var request = TournamentTemplateControllerTestData.CreateCreateTournamentTemplateRequest();
            _service.CreateAsyncResult = Result<TournamentTemplateResponse>.Failure(TournamentTemplateErrors.OrganizerNotFound);

            ActionResult<TournamentTemplateResponse> result = await _controller.CreateAsync(request, CancellationToken.None);

            var notFoundResult = result.Result.Should().BeOfType<NotFoundObjectResult>().Subject;
            notFoundResult.Value.Should().Be(TournamentTemplateErrors.OrganizerNotFound);
        }

        [Fact]
        public async Task CreateAsync_WithAlreadyExists_ReturnsConflict()
        {
            var request = TournamentTemplateControllerTestData.CreateCreateTournamentTemplateRequest();
            _service.CreateAsyncResult = Result<TournamentTemplateResponse>.Failure(TournamentTemplateErrors.AlreadyExists);

            ActionResult<TournamentTemplateResponse> result = await _controller.CreateAsync(request, CancellationToken.None);

            var conflictResult = result.Result.Should().BeOfType<ConflictObjectResult>().Subject;
            conflictResult.Value.Should().Be(TournamentTemplateErrors.AlreadyExists);
        }

        [Fact]
        public async Task GetByIdAsync_WithSuccess_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var response = TournamentTemplateControllerTestData.CreateTournamentTemplateResponse(id);
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
            _service.GetByIdAsyncResult = Result<TournamentTemplateResponse>.Failure(TournamentTemplateErrors.InvalidInput);

            ActionResult<TournamentTemplateResponse> result = await _controller.GetByIdAsync(id, CancellationToken.None);

            var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be(TournamentTemplateErrors.InvalidInput);
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
    }
}
