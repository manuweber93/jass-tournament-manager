using JassTournamentManager.Application.Auth;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Contracts.Users;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using Microsoft.Extensions.Options;

namespace JassTournamentManager.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserPasswordHasher _passwordHasher;
        private readonly IOptionsMonitor<AuthOptions> _authOptions;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IUserPasswordHasher passwordHasher, IOptionsMonitor<AuthOptions> authOptions)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _authOptions = authOptions ?? throw new ArgumentNullException(nameof(authOptions));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<Result<UserResponse>> CreateClaimableAsync(CreateUserRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
            {
                return Result<UserResponse>.Failure(CommonErrors.InvalidInput);
            }

            User user = new(null, null, request.FirstName, request.LastName, UserSourceType.Manual);
            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            UserResponse response = new(user.Id, user.Email, user.FirstName, user.LastName, user.IsSysAdmin);
            return Result<UserResponse>.Success(response);
        }

        public async Task<Result<UserResponse>> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            if (userId == Guid.Empty)
            {
                return Result<UserResponse>.Failure(CommonErrors.InvalidInput);
            }

            User? user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return Result<UserResponse>.Failure(UserErrors.NotFound);
            }

            UserResponse response = new(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.IsSysAdmin);
            return Result<UserResponse>.Success(response);
        }

        public async Task<Result<IEnumerable<ClaimableUserResponse>>> GetClaimableUsers(CancellationToken cancellationToken)
        {
            if (!_authOptions.CurrentValue.EnableUserClaiming)
            {
                return Result<IEnumerable<ClaimableUserResponse>>.Failure(AuthErrors.UserClaimingDisabled);
            }

            IReadOnlyCollection<User> claimableUsers = await _userRepository.GetClaimableUsersAsync(cancellationToken);

            IEnumerable<ClaimableUserResponse> claimableUsersResponse = claimableUsers.Select(
                user => new ClaimableUserResponse(user.Id, user.FirstName, user.LastName));

            return Result<IEnumerable<ClaimableUserResponse>>.Success(claimableUsersResponse);
        }

        public async Task<Result<bool>> ResetPassword(Guid userId, ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return Result<bool>.Failure(CommonErrors.InvalidInput);
            }

            User? user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
            {
                return Result<bool>.Failure(UserErrors.NotFound);
            }

            string passwordHash = _passwordHasher.HashPassword(request.NewPassword);
            user.SetPasswordHash(passwordHash);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
