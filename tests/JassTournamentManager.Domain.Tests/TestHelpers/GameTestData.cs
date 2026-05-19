using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class GameTestData
    {
        public static int CreateGameNumber() => 1;

        public static Game CreateGame() =>
            new(
                PairingTestData.CreatePairingId(),
                CreateGameNumber());

        public static int CreateGameScorePoints() => 100;

        public static GameScore CreateGameScore() => new(
            100,
            57,
            false,
            false,
            Guid.NewGuid());
    }
}
