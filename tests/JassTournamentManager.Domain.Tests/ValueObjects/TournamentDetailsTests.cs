using FluentAssertions;
using JassTournamentManager.Domain.Tests.TestHelpers;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Tests.ValueObjects
{
    public class TournamentDetailsTests
    {
        [Fact]
        public void Constructor_WithBlankName_ShouldThrowArgumentException()
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
        public void Constructor_WithBlankTournamentCode_ShouldThrowArgumentException()
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
        public void Constructor_WithDefaultDate_ShouldThrowArgumentException()
        {
            var defaultDate = default(DateOnly);

            Action act = () => new TournamentDetails(
                TournamentTestData.CreateTournamentName(),
                null,
                defaultDate,
                TournamentTestData.CreateTournamentCode());

            act.Should().Throw<ArgumentException>();
        }
    }
}
