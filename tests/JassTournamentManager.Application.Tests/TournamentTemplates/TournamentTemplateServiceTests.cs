using FluentAssertions;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.Tests.Common;
using JassTournamentManager.Application.Tests.TournamentConfigs;
using JassTournamentManager.Application.Tests.Users;
using JassTournamentManager.Application.TournamentConfigs;
using JassTournamentManager.Application.TournamentTemplates;
using JassTournamentManager.Contracts.TournamentConfigs;
using JassTournamentManager.Contracts.TournamentTemplates;
using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Tests.TournamentTemplates
{
    public class TournamentTemplateServiceTests
    {
        private readonly FakeUnitOfWork _unitOfWork;
        private readonly FakeTournamentTemplateRepository _tournamentTemplateRepository;
        private readonly FakeUserRepository _userRepository;
        private readonly TournamentTemplateService _tournamentTemplateService;

        public TournamentTemplateServiceTests()
        {
            _unitOfWork = new FakeUnitOfWork();
            _tournamentTemplateRepository = new FakeTournamentTemplateRepository();
            _userRepository = new FakeUserRepository();

            _tournamentTemplateService = new TournamentTemplateService(_tournamentTemplateRepository, _userRepository, _unitOfWork);
        }

        [Fact]
        public async Task CreateAsync_WithNullRequest_ThrowsArgumentNullException()
        {
            CreateTournamentTemplateRequest? nullRequest = null;

            Func<Task> act = () => _tournamentTemplateService.CreateAsync(nullRequest!, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateAsync_WithEmptyOrganizerId_ReturnsFailureResultWithInvalidInputError()
        {
            Guid emptyOrganizerId = Guid.Empty;
            CreateTournamentTemplateRequest request = new(
                emptyOrganizerId,
                TournamentTestData.CreateTournamentConfigDto(),
                TournamentTemplateTestData.CreateLocation());

            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.CreateAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TournamentTemplateErrors.InvalidInput);
            _tournamentTemplateRepository.TournamentTemplates.Should().BeEmpty();
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task CreateAsync_WithNonExistentOrganizerId_ReturnsFailureResultWithOrganizerNotFoundError()
        {
            CreateTournamentTemplateRequest request = TournamentTemplateTestData.CreateCreateTournamentTemplateRequest();

            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.CreateAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TournamentTemplateErrors.OrganizerNotFound);
            _tournamentTemplateRepository.TournamentTemplates.Should().BeEmpty();
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task CreateAsync_WithUserWithExistingTournamentTemplate_ReturnsFailureResultWithAlreadyExistsError()
        {
            User user = UserTestData.CreateUser();
            await _userRepository.AddAsync(user, CancellationToken.None);

            TournamentTemplate tournamentTemplate = TournamentTemplateTestData.CreateTournamentTemplate(user.Id);
            await _tournamentTemplateRepository.AddAsync(tournamentTemplate, CancellationToken.None);
            
            CreateTournamentTemplateRequest request = TournamentTemplateTestData.CreateCreateTournamentTemplateRequest(user.Id);
            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.CreateAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TournamentTemplateErrors.AlreadyExists);
            _tournamentTemplateRepository.TournamentTemplates.Count.Should().Be(1);
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task CreateAsync_WithNullConfig_ReturnsFailureResultWithInvalidInputError()
        {
            User user = UserTestData.CreateUser();
            await _userRepository.AddAsync(user, CancellationToken.None);

            CreateTournamentTemplateRequest request = TournamentTemplateTestData.CreateCreateTournamentTemplateRequestWithNullConfig(user.Id);
            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.CreateAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TournamentConfigErrors.InvalidInput);
            _tournamentTemplateRepository.TournamentTemplates.Count.Should().Be(0);
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidConfig_ReturnsFailureResultWithInvalidInputError()
        {
            User user = UserTestData.CreateUser();
            await _userRepository.AddAsync(user, CancellationToken.None);

            TournamentConfigDto config = TournamentTestData.CreateTournamentConfigDto(0);
            CreateTournamentTemplateRequest request = TournamentTemplateTestData.CreateCreateTournamentTemplateRequest(user.Id, config: config);
            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.CreateAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TournamentConfigErrors.InvalidInput);
            _tournamentTemplateRepository.TournamentTemplates.Count.Should().Be(0);
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task CreateAsync_WithTooLongLocation_ReturnsFailureResultWithInvalidInputError()
        {
            User user = UserTestData.CreateUser();
            await _userRepository.AddAsync(user, CancellationToken.None);

            CreateTournamentTemplateRequest request = TournamentTemplateTestData.CreateCreateTournamentTemplateRequest(
                user.Id,
                location: TournamentTemplateTestData.CreateTooLongLocation());
            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.CreateAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TournamentTemplateErrors.InvalidInput);
            _tournamentTemplateRepository.TournamentTemplates.Count.Should().Be(0);
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_CreatesTournamentTemplate()
        {
            User user = UserTestData.CreateUser();
            await _userRepository.AddAsync(user, CancellationToken.None);

            CreateTournamentTemplateRequest request = TournamentTemplateTestData.CreateCreateTournamentTemplateRequest(user.Id);
            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.CreateAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            _tournamentTemplateRepository.TournamentTemplates.Count.Should().Be(1);
            _unitOfWork.SaveChangesCallCount.Should().Be(1);
            result.Value.Config.Should().BeEquivalentTo(request.Config);
            result.Value.OrganizerId.Should().Be(user.Id);
            result.Value.Location.Should().Be(request.Location);
        }

        [Fact]
        public async Task GetByIdAsync_WithEmptyTournamentTemplateId_ReturnsFailureResultWithInvalidInputError()
        {
            Guid emptyId = Guid.Empty;

            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.GetByIdAsync(emptyId, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TournamentTemplateErrors.InvalidInput);
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentTournamentTemplateId_ReturnsFailureResultWithNotFoundError()
        {
            Guid nonExistentId = Guid.NewGuid();

            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.GetByIdAsync(nonExistentId, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(TournamentTemplateErrors.NotFound);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentIdTournamentTemplateId_ReturnSuccessResultWithTournamentTemplateResponse()
        {
            TournamentTemplate tournamentTemplate = TournamentTemplateTestData.CreateTournamentTemplate();
            await _tournamentTemplateRepository.AddAsync(tournamentTemplate, CancellationToken.None);

            Result<TournamentTemplateResponse> result = await _tournamentTemplateService.GetByIdAsync(tournamentTemplate.Id, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().Be(tournamentTemplate.Id);
            result.Value.OrganizerId.Should().Be(tournamentTemplate.OrganizerId);
            result.Value.Config.Should().BeEquivalentTo(TournamentConfigDtoMapper.ToDto(tournamentTemplate.ConfigValues));
            result.Value.Location.Should().Be(tournamentTemplate.Location);
        }
    }
}
