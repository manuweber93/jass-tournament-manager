using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.Users;
using JassTournamentManager.Contracts.Auth;
using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Application.Auth
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthService(IUserRepository userRepository, IUnitOfWork unitOfWork, IUserPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
        }

        public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (string.IsNullOrWhiteSpace(request.Email)
                || string.IsNullOrEmpty(request.Password)
                || string.IsNullOrEmpty(request.FirstName)
                || string.IsNullOrEmpty(request.LastName))
            {
                return Result<AuthResponse>.Failure(AuthErrors.InvalidInput);
            }

            if (request.ClaimedUserId is not null)
            {
                return await ClaimUser(request, cancellationToken);
            } else
            {
                return await RegisterNewUser(request, cancellationToken);
            }
        }

        private async Task<Result<AuthResponse>> ClaimUser(RegisterRequest request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetByIdAsync(request.ClaimedUserId!, cancellationToken);

            if (user is null)
            {
                return Result<AuthResponse>.Failure(AuthErrors.InvalidInput);
            }

            string passwordHash = _passwordHasher.HashPassword(user, request.Password);

            user.Update(request.Email, passwordHash, request.FirstName, request.LastName, user.IsActive);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            AuthResponse response = CreateAuthResponseForUser(user);
            return Result<AuthResponse>.Success(response);
        }

        
        
        private async Task<Result<AuthResponse>> RegisterNewUser(RegisterRequest request, CancellationToken cancellationToken)
        {
            bool isEmailAlreadyInUser = await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken);
            if (isEmailAlreadyInUser)
            {
                return Result<AuthResponse>.Failure(AuthErrors.EmailAlreadyInUse);
            }

        }


        // Register

        // Login

        // RefreshSession

        // Logout

        private AuthResponse CreateAuthResponseForUser(User user)
        {

        }



    }
}
