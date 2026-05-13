using FluentAssertions;
using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Domain.Tests.Entities
{
    public class TournamentTests
    {

        [Fact]
        public void Constructor_WithEmptyOrganizerId_ShouldThrowArgumentException()
        {
            // Arrange
            var emptyGuid = Guid.Empty;
            // Act
            Action act = () => new Tournament(emptyGuid, "Test Tournament", null, new DateOnly(2026, 5, 13), "TST2024");
            // Assert
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Constructor_WithBlankName_ShouldThrowArgumentException()
        {
            // Arrange
            var organizerId = Guid.NewGuid();
            // Act
            Action act = () => new Tournament(organizerId, "   ", null, new DateOnly(2026, 5, 13), "TST2024");
            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
