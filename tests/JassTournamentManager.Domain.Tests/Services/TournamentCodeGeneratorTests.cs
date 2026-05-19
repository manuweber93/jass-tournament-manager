using FluentAssertions;
using JassTournamentManager.Domain.Services;

namespace JassTournamentManager.Domain.Tests.Services
{
    public class TournamentCodeGeneratorTests
    {
        [Fact]
        public void GenerateTournamentCode_WithInvalidLength_ThrowsArgumentOutOfRangeException()
        {
            var invalidLength = 0;

            Action act = () => TournamentCodeGenerator.GenerateTournamentCode(invalidLength);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GenerateTournamentCode_WithCustomLength_CreatesCodeWithRequestedLength()
        {
            var codeLength = 10;

            var code = TournamentCodeGenerator.GenerateTournamentCode(codeLength);

            code.Should().HaveLength(codeLength);
        }

        [Fact]
        public void GenerateTournamentCode_CreatesCodeWithAllowedCharactersOnly()
        {
            var code = TournamentCodeGenerator.GenerateTournamentCode(100);

            code.All(TournamentCodeGenerator.AllowedChars.Contains).Should().BeTrue();
        }
    }
}
