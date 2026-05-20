using FluentAssertions;
using JassTournamentManager.Domain.Tests.TestHelpers;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Tests.ValueObjects
{
    public class TournamentDetailsTests
    {
        [Fact]
        public void Constructor_WithBlankName_ThrowsArgumentException()
        {
            var blankName = "   ";

            Action act = () => new TournamentDetails(
                blankName,
                null,
                TournamentTestData.CreateTournamentDate(),
                TournamentTestData.CreateTournamentCode());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithTooLongName_ThrowsArgumentException()
        {
            var tooLongName = "Internationales Frühlings-, Sommer-, Herbst- und Winter-" +
                                "Gedächtnis-Schieberjassturnier des Verbandes der traditionellen " +
                                "Jassvereine der gesamten deutschsprachigen Alpenregion mit " +
                                "Rahmenprogramm, Festwirtschaft, Livemusik und regionaler Delegation";

            Action act = () => new TournamentDetails(
                tooLongName,
                null,
                TournamentTestData.CreateTournamentDate(),
                TournamentTestData.CreateTournamentCode());

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Constructor_WithTooLongLocation_ThrowsArgumentException()
        {
            var tooLongLocation = "Mehrzweckhalle und Kulturzentrum Hinteroberunterwasserschlossensberg, " +
                                    "Panoramastrasse 145, Gebäude Süd-West, Veranstaltungs- und Kongressbereich " +
                                    "mit zusätzlicher Besuchertribüne, unterirdischer Parkgarage und historischem Festsaal";

            Action act = () => new TournamentDetails(
                TournamentTestData.CreateTournamentName(),
                tooLongLocation,
                TournamentTestData.CreateTournamentDate(),
                TournamentTestData.CreateTournamentCode());

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Constructor_WithBlankTournamentCode_ThrowsArgumentException()
        {
            var blankTournamentCode = "";

            Action act = () => new TournamentDetails(
                TournamentTestData.CreateTournamentName(),
                null,
                TournamentTestData.CreateTournamentDate(),
                blankTournamentCode);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithTooLongTournamentCode_ThrowsArgumentException()
        {
            var tooLongTournamentCode = "ABCDEF1234567890GHIJK";

            Action act = () => new TournamentDetails(
                TournamentTestData.CreateTournamentName(),
                TournamentTestData.CreateLocation(),
                TournamentTestData.CreateTournamentDate(),
                tooLongTournamentCode);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Constructor_WithDefaultDate_ThrowsArgumentException()
        {
            var defaultDate = default(DateOnly);

            Action act = () => new TournamentDetails(
                TournamentTestData.CreateTournamentName(),
                null,
                defaultDate,
                TournamentTestData.CreateTournamentCode());

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithValidValues_CreatesTournamentDetails()
        {
            var name = TournamentTestData.CreateTournamentName();
            var location = TournamentTestData.CreateLocation();
            var date = TournamentTestData.CreateTournamentDate();
            var code = TournamentTestData.CreateTournamentCode();

            var details = new TournamentDetails(
                name,
                location,
                date,
                code);

            details.Name.Should().Be(name);
            details.Location.Should().Be(location);
            details.Date.Should().Be(date);
            details.TournamentCode.Should().Be(code);
        }

        [Fact]
        public void Constructor_WithPaddedValues_NormalizesTournamentDetails()
        {
            var name = TournamentTestData.CreateTournamentName();
            var location = TournamentTestData.CreateLocation();
            var tournamentCode = TournamentTestData.CreateTournamentCode();

            var details = new TournamentDetails(
                " " + name + " ",
                " " + location + " ",
                TournamentTestData.CreateTournamentDate(),
                " " + tournamentCode + " ");

            details.Name.Should().Be(name);
            details.Location.Should().Be(location);
            details.TournamentCode.Should().Be(tournamentCode);
        }

        [Fact]
        public void Constructor_WithNullLocation_CreatesTournamentDetailsWithoutLocation()
        {
            var details = new TournamentDetails(
                TournamentTestData.CreateTournamentName(),
                null,
                TournamentTestData.CreateTournamentDate(),
                TournamentTestData.CreateTournamentCode());

            details.Location.Should().BeNull();
        }
    }
}
