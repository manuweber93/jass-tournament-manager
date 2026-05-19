using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Domain.Tests.TestHelpers
{
    internal static class GameTestData
    {
        public static int CreateGameNumber() => 1;

        public static bool CreateMatchBonusEnabled() => true;

        public static Game CreateGame() =>
            new(
                PairingTestData.CreatePairingId(),
                CreateGameNumber(),
                CreateMatchBonusEnabled());

        public static Game CreateGame(bool matchBonusEnabled) =>
            new(
                PairingTestData.CreatePairingId(),
                CreateGameNumber(),
                matchBonusEnabled);

        public static Game CreateGame(
            Guid pairingId,
            int gameNumber = 1,
            bool matchBonusEnabled = true,
            GameStatus status = GameStatus.Pending) =>
            new(
                pairingId,
                gameNumber,
                matchBonusEnabled,
                status);

        public static int CreateGameScorePoints() => 100;

        public static GameScore CreateGameScore() => new(
            100,
            57,
            false,
            false,
            Guid.NewGuid());
    }
}
