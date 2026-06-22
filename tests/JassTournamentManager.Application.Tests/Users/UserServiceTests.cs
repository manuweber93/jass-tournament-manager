using FluentAssertions;
using JassTournamentManager.Application.Auth;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.Tests.Auth;
using JassTournamentManager.Application.Tests.Common;
using JassTournamentManager.Application.Tests.TournamentTemplates;
using JassTournamentManager.Application.TournamentConfigs;
using JassTournamentManager.Application.TournamentTemplates;
using JassTournamentManager.Application.Users;
using JassTournamentManager.Contracts.TournamentTemplates;
using JassTournamentManager.Contracts.Users;
using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Tests.Users
{
    public class UserServiceTests
    {
        private readonly FakeUserRepository _userRepository;
        private readonly FakeUnitOfWork _unitOfWork;
        private readonly FakeUserPasswordHasher _passwordHasher;
        private readonly FakeAuthOptionsMonitor _authOptions;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepository = new FakeUserRepository();
            _unitOfWork = new FakeUnitOfWork();
            _passwordHasher = new FakeUserPasswordHasher();
            _authOptions = new FakeAuthOptionsMonitor(new AuthOptions());
            _userService = new UserService(_userRepository, _unitOfWork, _passwordHasher, _authOptions);
        }

        [Fact]
        public async Task CreateAsync_WithNullRequest_ThrowsArgumentNullException()
        {
            CreateUserRequest? nullRequest = null;

            Func<Task> act = () => _userService.CreateClaimableAsync(nullRequest!, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateAsync_WithEmptyFirstName_ReturnsInvalidInput()
        {
            var emptyFirstName = " ";
            var request = new CreateUserRequest(
                emptyFirstName,
                UserTestData.CreateLastName());

            Result<UserResponse> result = await _userService.CreateClaimableAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommonErrors.InvalidInput);
            _userRepository.Users.Should().BeEmpty();
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task CreateAsync_WithEmptyLastName_ReturnsInvalidInput()
        {
            var emptyLastName = " ";
            var request = new CreateUserRequest(
                UserTestData.CreateFirstName(),
                emptyLastName);

            Result<UserResponse> result = await _userService.CreateClaimableAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommonErrors.InvalidInput);
            _userRepository.Users.Should().BeEmpty();
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task CreateAsync_WithValidInput_CreatesUser()
        {
            CreateUserRequest request = UserTestData.CreateCreateUserRequest();

            Result<UserResponse> result = await _userService.CreateClaimableAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            _userRepository.Users.Count.Should().Be(1);
            _unitOfWork.SaveChangesCallCount.Should().Be(1);
            result.Value.FirstName.Should().BeEquivalentTo(request.FirstName);
            result.Value.LastName.Should().BeEquivalentTo(request.LastName);
            result.Value.IsSysAdmin.Should().BeFalse();
        }

        [Fact]
        public async Task GetByIdAsync_WithEmptyUserId_ReturnsInvalidInput()
        {
            Guid emptyId = Guid.Empty;

            Result<UserResponse> result = await _userService.GetByIdAsync(emptyId, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommonErrors.InvalidInput);
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentUserId_ReturnsNotFound()
        {
            Guid nonExistentId = Guid.NewGuid();

            Result<UserResponse> result = await _userService.GetByIdAsync(nonExistentId, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFound);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentUserId_ReturnUser()
        {
            User user = UserTestData.CreateUser();

            await _userRepository.AddAsync(user, CancellationToken.None);

            Result<UserResponse> result = await _userService.GetByIdAsync(user.Id, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().Be(user.Id);
            result.Value.Email.Should().Be(user.Email);
            result.Value.FirstName.Should().Be(user.FirstName);
            result.Value.LastName.Should().Be(user.LastName);
            result.Value.IsSysAdmin.Should().Be(user.IsSysAdmin);
        }

        [Fact]
        public async Task GetClaimableUsers_WithDeactivatedClaiming_ReturnsUserClaimingDisabled()
        {
            _authOptions.CurrentValue.EnableUserClaiming = false;
            var result = await _userService.GetClaimableUsers(CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AuthErrors.UserClaimingDisabled);
        }

        [Fact]
        public async Task GetClaimableUsers_WithActivatedClaiming_ReturnsClaimableUsers()
        {
            _authOptions.CurrentValue.EnableUserClaiming = true;
            await _userRepository.AddAsync(UserTestData.CreateClaimableUser(), CancellationToken.None);
            await _userRepository.AddAsync(UserTestData.CreateClaimableUser(), CancellationToken.None);

            var result = await _userService.GetClaimableUsers(CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Count().Should().Be(2);
            result.Value.First().FirstName.Should().BeEquivalentTo(UserTestData.CreateFirstName());
            result.Value.First().LastName.Should().BeEquivalentTo(UserTestData.CreateLastName());
        }

        [Fact]
        public async Task ResetPassword_WithNullRequest_ThrowsArgumentNullException()
        {
            ResetPasswordRequest? nullRequest = null;

            Func<Task> act = () => _userService.ResetPassword(UserTestData.CreateUserId(), nullRequest!, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task ResetPassword_WithEmptyPassword_ReturnsInvalidInput()
        {
            var emptyPassword = " ";
            ResetPasswordRequest request = new(emptyPassword);

            var result = await _userService.ResetPassword(UserTestData.CreateUserId(), request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommonErrors.InvalidInput);
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task ResetPassword_WithNonExistentUser_ReturnsNotFound()
        {
            ResetPasswordRequest request = new(UserTestData.CreatePassword());

            var result = await _userService.ResetPassword(UserTestData.CreateUserId(), request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(UserErrors.NotFound);
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task ResetPassword_WithValidInput_ResetsPassword()
        {
            User user = UserTestData.CreateUser();
            await _userRepository.AddAsync(user, CancellationToken.None);
            var newPassword = "new-password";
            ResetPasswordRequest request = new(newPassword);

            var result = await _userService.ResetPassword(user.Id, request, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeTrue();
            user.PasswordHash.Should().Be(_passwordHasher.HashPassword(newPassword));
            _unitOfWork.SaveChangesCallCount.Should().Be(1);
        }
    }
}
