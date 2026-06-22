using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.Users;
using JassTournamentManager.Contracts.Auth;
using JassTournamentManager.Contracts.Users;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using Microsoft.Extensions.Options;

namespace JassTournamentManager.Application.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IOptionsMonitor<AuthOptions> _authOptions;

        public AuthService(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            IUserPasswordHasher passwordHasher,
            ITokenGenerator tokenGenerator,
            IOptionsMonitor<AuthOptions> authOptions)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            _authOptions = authOptions ?? throw new ArgumentNullException(nameof(authOptions));
        }

        public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.Email)
                || string.IsNullOrWhiteSpace(request.Password)
                || string.IsNullOrWhiteSpace(request.FirstName)
                || string.IsNullOrWhiteSpace(request.LastName))
            {
                return Result<AuthResponse>.Failure(CommonErrors.InvalidInput);
            }

            if (request.ClaimedUserId is not null)
            {
                return await ClaimExistingUser(request, cancellationToken);
            }
            else
            {
                return await RegisterNewUser(request, cancellationToken);
            }
        }

        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Result<AuthResponse>.Failure(CommonErrors.InvalidInput);
            }

            User? user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user is null || !user.CanLogin())
            {
                return Result<AuthResponse>.Failure(CommonErrors.InvalidInput);
            }

            bool isPasswordCorrect = _passwordHasher.VerifyPassword(user.PasswordHash!, request.Password);
            if (!isPasswordCorrect)
            {
                return Result<AuthResponse>.Failure(CommonErrors.InvalidInput);
            }

            IssuedRefreshToken refreshToken = await IssueRefreshTokenAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            AuthResponse response = CreateAuthResponseForUser(user, refreshToken.Secret, cancellationToken);
            return Result<AuthResponse>.Success(response);
        }

        public async Task<Result<AuthResponse>> RefreshSessionAsync(RefreshSessionRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            string refreshTokenHash = _tokenGenerator.HashRefreshToken(request.RefreshToken);
            RefreshToken? oldRefreshToken = await _refreshTokenRepository.GetByHashAsync(refreshTokenHash, cancellationToken);
            if (oldRefreshToken is null || !oldRefreshToken.IsActive)
            {
                return Result<AuthResponse>.Failure(CommonErrors.InvalidInput);
            }

            User? user = await _userRepository.GetByIdAsync(oldRefreshToken.UserId, cancellationToken);
            if (user is null || !user.CanLogin())
            {
                return Result<AuthResponse>.Failure(CommonErrors.InvalidInput);
            }

            IssuedRefreshToken newRefreshToken = await IssueRefreshTokenAsync(user, cancellationToken);
            oldRefreshToken.Replace(newRefreshToken.Entity.Id);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            AuthResponse response = CreateAuthResponseForUser(user, newRefreshToken.Secret, cancellationToken);
            return Result<AuthResponse>.Success(response);
        }

        public async Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            string refreshTokenHash = _tokenGenerator.HashRefreshToken(request.RefreshToken);
            RefreshToken? refreshToken = await _refreshTokenRepository.GetByHashAsync(refreshTokenHash, cancellationToken);
            if (refreshToken is null || !refreshToken.IsActive)
            {
                return;
            }

            refreshToken.Revoke();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<Result<AuthResponse>> ClaimExistingUser(RegisterRequest request, CancellationToken cancellationToken)
        {
            if (!_authOptions.CurrentValue.EnableUserClaiming)
            {
                return Result<AuthResponse>.Failure(AuthErrors.UserClaimingDisabled);
            }

            User? userById = await _userRepository.GetByIdAsync((Guid)request.ClaimedUserId!, cancellationToken);
            if (userById is null || !userById.CanBeClaimed())
            {
                return Result<AuthResponse>.Failure(CommonErrors.InvalidInput);
            }

            User? userByEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (userByEmail is not null && userById.Id != userByEmail.Id)
            {
                return Result<AuthResponse>.Failure(AuthErrors.EmailAlreadyInUse);
            }

            string passwordHash = _passwordHasher.HashPassword(request.Password);
            userById.Claim(request.Email, passwordHash, request.FirstName, request.LastName);

            IssuedRefreshToken refreshToken = await IssueRefreshTokenAsync(userById, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            AuthResponse response = CreateAuthResponseForUser(userById, refreshToken.Secret, cancellationToken);
            return Result<AuthResponse>.Success(response);
        }

        private async Task<Result<AuthResponse>> RegisterNewUser(RegisterRequest request, CancellationToken cancellationToken)
        {
            bool isEmailAlreadyInUse = await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken);
            if (isEmailAlreadyInUse)
            {
                return Result<AuthResponse>.Failure(AuthErrors.EmailAlreadyInUse);
            }

            string passwordHash = _passwordHasher.HashPassword(request.Password);
            User user = new(request.Email, passwordHash, request.FirstName, request.LastName, UserSourceType.SelfRegistered);
            await _userRepository.AddAsync(user, cancellationToken);

            IssuedRefreshToken refreshToken = await IssueRefreshTokenAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            AuthResponse response = CreateAuthResponseForUser(user, refreshToken.Secret, cancellationToken);
            return Result<AuthResponse>.Success(response);
        }

        private AuthResponse CreateAuthResponseForUser(User user, RefreshTokenSecret generatedRefreshToken, CancellationToken cancellationToken)
        {
            var generatedAccessToken = _tokenGenerator.GenerateAccessTokenSecret(user);

            var currentUserResponse = new UserResponse(
                user.Id,
                user.Email!,
                user.FirstName,
                user.LastName,
                user.IsSysAdmin);

            return new AuthResponse(
                generatedAccessToken.Token,
                generatedAccessToken.ExpiresAt,
                generatedRefreshToken.Token,
                generatedRefreshToken.ExpiresAt,
                currentUserResponse);
        }


        private async Task<IssuedRefreshToken> IssueRefreshTokenAsync(User user, CancellationToken cancellationToken)
        {
            RefreshTokenSecret refreshTokenSecret = _tokenGenerator.GenerateRefreshTokenSecret();

            var refreshTokenEntity = new RefreshToken(
                user.Id,
                refreshTokenSecret.TokenHash,
                refreshTokenSecret.ExpiresAt);

            await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

            return new IssuedRefreshToken(refreshTokenSecret, refreshTokenEntity);
        }

        private sealed record IssuedRefreshToken(
            RefreshTokenSecret Secret,
            RefreshToken Entity);
    }
}