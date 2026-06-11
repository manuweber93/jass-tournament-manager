using JassTournamentManager.Application.Common;
using JassTournamentManager.Contracts.Users;

namespace JassTournamentManager.Application.Users
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // Create Async (for user created by organizer)
        public async Task<CurrentUserResponse> CreateAsync()
        {
            throw new NotImplementedException();
        }


        // Claimable users

        // ResetPassword

    }
}
