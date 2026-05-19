using JassTournamentManager.Domain.Entities;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class PairingTestData
    {
        public static Guid CreatePairingId() => Guid.NewGuid();

        public static Pairing CreatePairing() => new(
            RoundTestData.CreateRoundId(),
            JassTableTestData.CreateJassTableId());
    }
}
