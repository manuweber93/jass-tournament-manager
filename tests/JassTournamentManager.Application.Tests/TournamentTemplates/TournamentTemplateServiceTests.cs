using FluentAssertions;
using JassTournamentManager.Application.Tests.Fakes;
using JassTournamentManager.Application.Tests.Users;
using JassTournamentManager.Application.TournamentTemplates;
using JassTournamentManager.Contracts.TournamentTemplates;

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

        // TODO: Tests for the following scenarios:
        //CreateAsync gibt Fehler, wenn OrganizerId leer ist
        //CreateAsync gibt OrganizerNotFound, wenn User nicht existiert
        //CreateAsync gibt AlreadyExists, wenn Organizer schon ein Template hat
        //CreateAsync gibt InvalidInput, wenn Config ungültig oder null ist
        //CreateAsync erstellt Template, ruft AddAsync und SaveChangesAsync auf
        //GetByIdAsync gibt InvalidInput, wenn Id leer ist
        //GetByIdAsync gibt NotFound, wenn Repository nichts findet
        //GetByIdAsync gibt Response, wenn Template existiert


    }
}
