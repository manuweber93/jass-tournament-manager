using JassTournamentManager.Application.Auth;
using JassTournamentManager.Application.Common;
using JassTournamentManager.Application.TournamentConfigs;
using JassTournamentManager.Application.Users;
using JassTournamentManager.Contracts.TournamentConfigs;
using JassTournamentManager.Contracts.TournamentTemplates;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Application.TournamentTemplates
{
    public class TournamentTemplateService : ITournamentTemplateService
    {
        private readonly ICurrentUser _currentUser;
        private readonly IUserRepository _userRepository;
        private readonly ITournamentTemplateRepository _tournamentTemplateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TournamentTemplateService(
            ICurrentUser currentUser,
            ITournamentTemplateRepository tournamentTemplateRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _tournamentTemplateRepository = tournamentTemplateRepository ?? throw new ArgumentNullException(nameof(tournamentTemplateRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        private bool IsCurrentUserAuthenticated => _currentUser.IsAuthenticated && _currentUser.UserId != Guid.Empty;

        public async Task<Result<TournamentTemplateResponse>> CreateAsync(
            CreateTournamentTemplateRequest request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (!IsCurrentUserAuthenticated)
            {
                return Result<TournamentTemplateResponse>.Failure(AuthErrors.Unauthorized);
            }

            Result organizerValidationResult = await ValidateOrganizerCanCreateTemplateAsync(_currentUser.UserId, cancellationToken);
            if (organizerValidationResult.IsFailure)
            {
                return Result<TournamentTemplateResponse>.Failure(organizerValidationResult.Error);
            }

            Result<TournamentConfigValues> configValuesResult = CreateConfigValues(request.Config);
            if (configValuesResult.IsFailure)
            {
                return Result<TournamentTemplateResponse>.Failure(configValuesResult.Error);
            }

            Result<TournamentTemplate> tournamentTemplateResult = CreateTournamentTemplate(_currentUser.UserId, configValuesResult.Value, request.Location);
            if (tournamentTemplateResult.IsFailure)
            {
                return Result<TournamentTemplateResponse>.Failure(tournamentTemplateResult.Error);
            }

            TournamentTemplate tournamentTemplate = tournamentTemplateResult.Value;

            await _tournamentTemplateRepository.AddAsync(tournamentTemplate, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            TournamentTemplateResponse response = CreateResponseForTournamentTemplate(tournamentTemplate);
            return Result<TournamentTemplateResponse>.Success(response);
        }

        public async Task<Result<TournamentTemplateResponse>> GetByIdAsync(Guid tournamentTemplateId, CancellationToken cancellationToken)
        {
            if (!IsCurrentUserAuthenticated)
            {
                return Result<TournamentTemplateResponse>.Failure(AuthErrors.Unauthorized);
            }

            if (tournamentTemplateId == Guid.Empty)
            {
                return Result<TournamentTemplateResponse>.Failure(CommonErrors.InvalidInput);
            }

            TournamentTemplate? tournamentTemplate = await _tournamentTemplateRepository.GetByIdAsync(tournamentTemplateId, cancellationToken);
            if (tournamentTemplate is null)
            {
                return Result<TournamentTemplateResponse>.Failure(TournamentTemplateErrors.NotFound);
            }

            if (tournamentTemplate.OrganizerId != _currentUser.UserId && !_currentUser.IsSysAdmin)
            {
                return Result<TournamentTemplateResponse>.Failure(CommonErrors.Forbidden);
            }

            TournamentTemplateResponse response = CreateResponseForTournamentTemplate(tournamentTemplate);
            return Result<TournamentTemplateResponse>.Success(response);
        }

        public async Task<Result<TournamentTemplateResponse>> GetForCurrentUserAsync(CancellationToken cancellationToken)
        {
            if (!IsCurrentUserAuthenticated)
            {
                return Result<TournamentTemplateResponse>.Failure(AuthErrors.Unauthorized);
            }

            TournamentTemplate? tournamentTemplate = await _tournamentTemplateRepository.GetByOrganizerIdAsync(_currentUser.UserId, cancellationToken);
            if (tournamentTemplate is null)
            {
                return Result<TournamentTemplateResponse>.Failure(TournamentTemplateErrors.NotFound);
            }

            TournamentTemplateResponse response = CreateResponseForTournamentTemplate(tournamentTemplate);
            return Result<TournamentTemplateResponse>.Success(response);
        }

        private async Task<Result> ValidateOrganizerCanCreateTemplateAsync(Guid organizerId, CancellationToken cancellationToken)
        {
            if (organizerId == Guid.Empty)
            {
                return Result.Failure(CommonErrors.InvalidInput);
            }

            if (!await _userRepository.ExistsAsync(organizerId, cancellationToken))
            {
                return Result.Failure(TournamentTemplateErrors.OrganizerNotFound);
            }

            if (await _tournamentTemplateRepository.ExistsForOrganizerAsync(organizerId, cancellationToken))
            {
                return Result.Failure(TournamentTemplateErrors.AlreadyExists);
            }

            return Result.Success();
        }

        private static Result<TournamentConfigValues> CreateConfigValues(TournamentConfigDto? config)
        {
            if (config is null)
            {
                return Result<TournamentConfigValues>.Failure(CommonErrors.InvalidInput);
            }

            try
            {
                TournamentConfigValues configValues = TournamentConfigDtoMapper.FromDto(config);
                return Result<TournamentConfigValues>.Success(configValues);
            }
            catch (ArgumentException)
            {
                return Result<TournamentConfigValues>.Failure(CommonErrors.InvalidInput);
            }
        }

        private static Result<TournamentTemplate> CreateTournamentTemplate(Guid organizerId, TournamentConfigValues configValues, string? location)
        {
            try
            {
                TournamentTemplate tournamentTemplate = new(organizerId, configValues, location);
                return Result<TournamentTemplate>.Success(tournamentTemplate);
            }
            catch (ArgumentException)
            {
                return Result<TournamentTemplate>.Failure(CommonErrors.InvalidInput);
            }
        }

        private static TournamentTemplateResponse CreateResponseForTournamentTemplate(TournamentTemplate tournamentTemplate)
        {
            ArgumentNullException.ThrowIfNull(tournamentTemplate);

            return new TournamentTemplateResponse
            (
                tournamentTemplate.Id,
                tournamentTemplate.OrganizerId,
                TournamentConfigDtoMapper.ToDto(tournamentTemplate.ConfigValues),
                tournamentTemplate.Location,
                tournamentTemplate.CreatedAt,
                tournamentTemplate.UpdatedAt
            );
        }
    }
}
