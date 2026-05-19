using FluentAssertions;
using JassTournamentManager.Domain.Common;

namespace JassTournamentManager.Domain.Tests.Common
{
    public class GuardTests
    {
        [Fact]
        public void AgainstEmptyGuid_WithEmptyGuid_ThrowsArgumentException()
        {
            var emptyGuid = Guid.Empty;

            Action act = () => Guard.AgainstEmptyGuid(emptyGuid, nameof(emptyGuid));

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AgainstDefaultDateOnly_WithDefaultDateOnly_ThrowsArgumentException()
        {
            DateOnly defaultDate = default;

            Action act = () => Guard.AgainstDefaultDateOnly(defaultDate, nameof(defaultDate));

            act.Should().Throw<ArgumentException>();
        }
    }
}
