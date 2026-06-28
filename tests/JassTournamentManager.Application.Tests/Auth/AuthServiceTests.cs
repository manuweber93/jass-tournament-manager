using FluentAssertions;
using JassTournamentManager.Application.Auth;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.Tests.Common;
using JassTournamentManager.Application.Tests.Users;
using JassTournamentManager.Contracts.Auth;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;

namespace JassTournamentManager.Application.Tests.Auth
{
    public class AuthServiceTests
    {
        private readonly FakeUserRepository _userRepository;
        private readonly FakeRefreshTokenRepository _refreshTokenRepository;
        private readonly FakeUnitOfWork _unitOfWork;
        private readonly FakeUserPasswordHasher _passwordHasher;
        private readonly FakeTokenGenerator _tokenGenerator;
        private readonly FakeAuthOptionsMonitor _authOptions;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepository = new FakeUserRepository();
            _refreshTokenRepository = new FakeRefreshTokenRepository();
            _unitOfWork = new FakeUnitOfWork();
            _passwordHasher = new FakeUserPasswordHasher();
            _tokenGenerator = new FakeTokenGenerator();
            _authOptions = new FakeAuthOptionsMonitor(new AuthOptions());
            _authService = new AuthService(
                _userRepository,
                _refreshTokenRepository,
                _unitOfWork,
                _passwordHasher,
                _tokenGenerator,
                _authOptions);
        }

        [Fact]
        public async Task RegisterAsync_WithNewUser_CreatesUserAndReturnsTokens()
        {
            var request = AuthServiceTestData.CreateNewUserRegisterRequest();

            var result = await _authService.RegisterAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            _userRepository.Users.Should().ContainSingle();
            var createdUser = _userRepository.Users.Single();
            createdUser.Email.Should().Be(request.Email);
            createdUser.PasswordHash.Should().Be(_passwordHasher.PasswordHash);
            createdUser.FirstName.Should().Be(request.FirstName);
            createdUser.LastName.Should().Be(request.LastName);
            createdUser.SourceType.Should().Be(UserSourceType.SelfRegistered);
            AssertSuccessfulAuthResponse(result.Value, createdUser);
            AssertIssuedRefreshToken(createdUser);
            _unitOfWork.SaveChangesCallCount.Should().Be(1);
        }

        [Fact]
        public async Task RegisterAsync_WithInvalidEmailAddress_ReturnsInvalidEmailAddress()
        {
            var request = AuthServiceTestData.CreateRegisterRequest(null, email: AuthServiceTestData.CreateInvalidEmail());

            var result = await _authService.RegisterAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AuthErrors.InvalidEmailAddress);
            _refreshTokenRepository.RefreshTokens.Should().BeEmpty();
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task RegisterAsync_WithPasswordRequirementsNotMet_ReturnsPasswordRequirementsNotMet()
        {
            var request = AuthServiceTestData.CreateRegisterRequest(null, password: AuthServiceTestData.CreateWeakPassword());

            var result = await _authService.RegisterAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AuthErrors.PasswordRequirementsNotMet);
            _refreshTokenRepository.RefreshTokens.Should().BeEmpty();
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ReturnsEmailAlreadyInUse()
        {
            var existingUser = AuthServiceTestData.CreateLoginUser(AuthServiceTestData.CreateExistingEmail());
            await _userRepository.AddAsync(existingUser, CancellationToken.None);
            var request = AuthServiceTestData.CreateRegisterRequest(
                claimedUserId: null,
                email: existingUser.Email);

            var result = await _authService.RegisterAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AuthErrors.EmailAlreadyInUse);
            _refreshTokenRepository.RefreshTokens.Should().BeEmpty();
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task RegisterAsync_WithClaimableUserAndDeactivatedClaiming_ReturnsUserClaimingDisabled()
        {
            _authOptions.CurrentValue.EnableUserClaiming = false;
            var claimableUser = AuthServiceTestData.CreateClaimableUser();
            await _userRepository.AddAsync(claimableUser, CancellationToken.None);
            var request = AuthServiceTestData.CreateClaimExistingUserRequest(claimableUser.Id);

            var result = await _authService.RegisterAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AuthErrors.UserClaimingDisabled);
            _refreshTokenRepository.RefreshTokens.Should().BeEmpty();
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task RegisterAsync_WithClaimableUserAndActivatedClaiming_ClaimsUserAndReturnsTokens()
        {
            _authOptions.CurrentValue.EnableUserClaiming = true;
            var claimableUser = AuthServiceTestData.CreateClaimableUser();
            await _userRepository.AddAsync(claimableUser, CancellationToken.None);
            var request = AuthServiceTestData.CreateClaimExistingUserRequest(claimableUser.Id);

            var result = await _authService.RegisterAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            claimableUser.Email.Should().Be(request.Email);
            claimableUser.PasswordHash.Should().Be(_passwordHasher.PasswordHash);
            claimableUser.FirstName.Should().Be(request.FirstName);
            claimableUser.LastName.Should().Be(request.LastName);
            AssertSuccessfulAuthResponse(result.Value, claimableUser);
            AssertIssuedRefreshToken(claimableUser);
            _unitOfWork.SaveChangesCallCount.Should().Be(1);
        }

        [Fact]
        public async Task RegisterAsync_WithClaimedUserEmailUsedByDifferentUser_ReturnsAlreadyExists()
        {
            _authOptions.CurrentValue.EnableUserClaiming = true;
            var claimableUser = AuthServiceTestData.CreateClaimableUser();
            var existingUser = AuthServiceTestData.CreateLoginUser(AuthServiceTestData.CreateUsedEmail());
            await _userRepository.AddAsync(claimableUser, CancellationToken.None);
            await _userRepository.AddAsync(existingUser, CancellationToken.None);
            var request = AuthServiceTestData.CreateRegisterRequest(
                claimableUser.Id,
                email: existingUser.Email);

            var result = await _authService.RegisterAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(AuthErrors.EmailAlreadyInUse);
            _refreshTokenRepository.RefreshTokens.Should().BeEmpty();
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ReturnsTokens()
        {
            var user = AuthServiceTestData.CreateLoginUser();
            await _userRepository.AddAsync(user, CancellationToken.None);
            var request = AuthServiceTestData.CreateLoginRequest(user);

            var result = await _authService.LoginAsync(request, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            AssertSuccessfulAuthResponse(result.Value, user);
            AssertIssuedRefreshToken(user);
            _unitOfWork.SaveChangesCallCount.Should().Be(1);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ReturnsInvalidInput()
        {
            var user = AuthServiceTestData.CreateLoginUser();
            await _userRepository.AddAsync(user, CancellationToken.None);
            _passwordHasher.IsPasswordValid = false;
            var request = AuthServiceTestData.CreateLoginRequest(user, AuthServiceTestData.CreateWrongPassword());

            var result = await _authService.LoginAsync(request, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommonErrors.InvalidInput);
            _refreshTokenRepository.RefreshTokens.Should().BeEmpty();
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task RefreshSessionAsync_WithActiveRefreshToken_RotatesTokenAndReturnsNewTokens()
        {
            var user = AuthServiceTestData.CreateLoginUser();
            var oldRefreshToken = AuthServiceTestData.CreateActiveRefreshToken(user.Id, _tokenGenerator.RefreshTokenHash);
            await _userRepository.AddAsync(user, CancellationToken.None);
            await _refreshTokenRepository.AddAsync(oldRefreshToken, CancellationToken.None);

            var result = await _authService.RefreshSessionAsync(
                AuthServiceTestData.CreateRefreshSessionRequest(_tokenGenerator.RefreshToken),
                CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            AssertSuccessfulAuthResponse(result.Value, user);
            _refreshTokenRepository.RefreshTokens.Should().HaveCount(2);
            var newRefreshToken = _refreshTokenRepository.RefreshTokens.Single(token => token.Id != oldRefreshToken.Id);
            newRefreshToken.UserId.Should().Be(user.Id);
            newRefreshToken.TokenHash.Should().Be(_tokenGenerator.RefreshTokenHash);
            oldRefreshToken.IsRevoked.Should().BeTrue();
            oldRefreshToken.ReplacedByTokenId.Should().Be(newRefreshToken.Id);
            _unitOfWork.SaveChangesCallCount.Should().Be(1);
        }

        [Fact]
        public async Task RefreshSessionAsync_WithMissingRefreshToken_ReturnsInvalidInput()
        {
            var result = await _authService.RefreshSessionAsync(
                AuthServiceTestData.CreateRefreshSessionRequest(AuthServiceTestData.CreateMissingRefreshToken()),
                CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CommonErrors.InvalidInput);
            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task LogoutAsync_WithActiveRefreshToken_RevokesToken()
        {
            var user = AuthServiceTestData.CreateLoginUser();
            var refreshToken = AuthServiceTestData.CreateActiveRefreshToken(user.Id, _tokenGenerator.RefreshTokenHash);
            await _refreshTokenRepository.AddAsync(refreshToken, CancellationToken.None);

            await _authService.LogoutAsync(
                AuthServiceTestData.CreateLogoutRequest(_tokenGenerator.RefreshToken),
                CancellationToken.None);

            refreshToken.IsRevoked.Should().BeTrue();
            refreshToken.IsActive.Should().BeFalse();
            _unitOfWork.SaveChangesCallCount.Should().Be(1);
        }

        [Fact]
        public async Task LogoutAsync_WithRevokedRefreshToken_DoesNothing()
        {
            var user = AuthServiceTestData.CreateLoginUser();
            var refreshToken = AuthServiceTestData.CreateRevokedRefreshToken(user.Id, _tokenGenerator.RefreshTokenHash);
            await _refreshTokenRepository.AddAsync(refreshToken, CancellationToken.None);

            await _authService.LogoutAsync(
                AuthServiceTestData.CreateLogoutRequest(_tokenGenerator.RefreshToken),
                CancellationToken.None);

            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        [Fact]
        public async Task LogoutAsync_WithMissingRefreshToken_DoesNothing()
        {
            await _authService.LogoutAsync(
                AuthServiceTestData.CreateLogoutRequest(AuthServiceTestData.CreateMissingRefreshToken()),
                CancellationToken.None);

            _unitOfWork.SaveChangesCallCount.Should().Be(0);
        }

        private void AssertSuccessfulAuthResponse(AuthResponse response, User user)
        {
            response.AccessToken.Should().Be(_tokenGenerator.AccessToken);
            response.AccessTokenExpiresAt.Should().Be(_tokenGenerator.AccessTokenExpiresAt);
            response.RefreshToken.Should().Be(_tokenGenerator.RefreshToken);
            response.RefreshTokenExpiresAt.Should().Be(_tokenGenerator.RefreshTokenExpiresAt);
            response.User.Id.Should().Be(user.Id);
            response.User.Email.Should().Be(user.Email);
            response.User.FirstName.Should().Be(user.FirstName);
            response.User.LastName.Should().Be(user.LastName);
            response.User.IsSysAdmin.Should().Be(user.IsSysAdmin);
        }

        private void AssertIssuedRefreshToken(User user)
        {
            _refreshTokenRepository.RefreshTokens.Should().ContainSingle();
            var refreshToken = _refreshTokenRepository.RefreshTokens.Single();
            refreshToken.UserId.Should().Be(user.Id);
            refreshToken.TokenHash.Should().Be(_tokenGenerator.RefreshTokenHash);
            refreshToken.ExpiresAtUtc.Should().Be(_tokenGenerator.RefreshTokenExpiresAt);
        }

    }
}
