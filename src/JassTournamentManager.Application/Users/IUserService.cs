using JassTournamentManager.Application.Common;
using JassTournamentManager.Contracts.Users;

namespace JassTournamentManager.Application.Users
{
    public interface IUserService
    {
        Task<Result<UserResponse>> CreateClaimableAsync(CreateUserRequest request, CancellationToken cancellationToken);

        Task<Result<IEnumerable<ClaimableUserResponse>>> GetClaimableUsers(CancellationToken cancellationToken);

        Task<Result<bool>> ResetPassword(Guid userId, ResetPasswordRequest request, CancellationToken cancellationToken);

        Task<Result<UserResponse>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}