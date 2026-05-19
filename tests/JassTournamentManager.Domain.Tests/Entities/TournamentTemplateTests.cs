using FluentAssertions;
using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Tests.TestHelpers;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class TournamentTemplateTests
    {
        [Fact]
        public void Constructor_WithEmptyOrganizerId_ThrowsArgumentException()
        {
            var emptyOrganizerId = Guid.Empty;

            Action act = () => new TournamentTemplate(
                emptyOrganizerId,
                TournamentTestData.CreateTournamentConfigValues(),
                TournamentTestData.CreateLocation());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithNullConfigValues_ThrowsArgumentNullException()
        {
            Action act = () => new TournamentTemplate(
                UserTestData.CreateUserId(),
                null!,
                TournamentTestData.CreateLocation());

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesTournamentTemplate()
        {
            var organizerId = UserTestData.CreateUserId();
            var configValues = TournamentTestData.CreateTournamentConfigValues();
            var location = TournamentTestData.CreateLocation();

            var template = new TournamentTemplate(organizerId, configValues, location);

            template.OrganizerId.Should().Be(organizerId);
            template.ConfigValues.Should().Be(configValues);
            template.Location.Should().Be(location);
        }

        [Fact]
        public void UpdateLocation_WithNullLocation_ClearsLocation()
        {
            var template = TournamentTemplateTestData.CreateTournamentTemplate();

            template.UpdateLocation(null);

            template.Location.Should().BeNull();
        }

        [Fact]
        public void UpdateLocation_WithNewLocation_UpdatesLocation()
        {
            var template = TournamentTemplateTestData.CreateTournamentTemplate();
            var newLocation = "New Location";

            template.UpdateLocation(newLocation);

            template.Location.Should().Be(newLocation);
        }

        [Fact]
        public void UpdateConfig_WithNullConfigValues_ThrowsArgumentNullException()
        {
            var template = TournamentTemplateTestData.CreateTournamentTemplate();

            Action act = () => template.UpdateConfig(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UpdateConfig_WithValidConfigValues_UpdatesConfigValues()
        {
            var template = TournamentTemplateTestData.CreateTournamentTemplate();
            var configValues = new TournamentConfigValues(numberOfRounds: 7, gamesPerRound: 10);

            template.UpdateConfig(configValues);

            template.ConfigValues.Should().Be(configValues);
        }
    }
}
