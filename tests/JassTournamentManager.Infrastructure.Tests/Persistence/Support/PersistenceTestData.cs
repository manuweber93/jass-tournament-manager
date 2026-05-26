using JassTournamentManager.Domain.Entities;
using JassTournamentManager.Domain.Enums;
using JassTournamentManager.Domain.ValueObjects;

namespace JassTournamentManager.Infrastructure.Tests.Persistence
{
    internal static class PersistenceTestData
    {
        public static User CreateUser(
            string? email = null,
            string firstName = "Test",
            string lastName = "User",
            UserSourceType sourceType = UserSourceType.Manual)
        {
            return new User(
                email,
                passwordHash: null,
                firstName,
                lastName,
                sourceType);
        }

        public static User CreateSelfRegisteredUser(
            string email,
            string firstName = "Self",
            string lastName = "Registered")
        {
            return new User(
                email,
                "hashed-password",
                firstName,
                lastName,
                UserSourceType.SelfRegistered);
        }

        public static TournamentDetails CreateTournamentDetails(
            string tournamentCode,
            string name = "Spring Tournament")
        {
            return new TournamentDetails(
                name,
                "Club House",
                new DateOnly(2026, 5, 21),
                tournamentCode);
        }

        public static TournamentConfigValues CreateTournamentConfigValues()
        {
            return new TournamentConfigValues(
                numberOfRounds: 3,
                gamesPerRound: 2,
                matchBonusEnabled: true,
                isFixedTeams: false,
                scoreVisibility: ScoreVisibility.HiddenDuringActiveTournament);
        }

        public static Tournament CreateTournament(Guid organizerId, string tournamentCode)
        {
            return new Tournament(
                organizerId,
                CreateTournamentDetails(tournamentCode),
                CreateTournamentConfigValues());
        }

        public static TournamentTemplate CreateTournamentTemplate(Guid organizerId)
        {
            return new TournamentTemplate(
                organizerId,
                CreateTournamentConfigValues(),
                "Club House");
        }

        public static TournamentParticipant CreateTournamentParticipant(
            Guid tournamentId,
            Guid userId,
            ParticipantRole role = ParticipantRole.Player)
        {
            return new TournamentParticipant(
                tournamentId,
                userId,
                RegistrationMethod.ByOrganizer,
                role);
        }

        public static Round CreateRound(Guid tournamentId, int roundNumber = 1)
        {
            return new Round(tournamentId, roundNumber);
        }

        public static JassTable CreateJassTable(Guid organizerId, int displayOrder = 0)
        {
            return new JassTable(organizerId, $"Table {displayOrder + 1}", displayOrder);
        }

        public static Pairing CreatePairing(Guid roundId, Guid jassTableId, int gamesPerRound = 2)
        {
            return new Pairing(roundId, jassTableId, gamesPerRound);
        }

        public static PairingParticipant CreatePairingParticipant(
            Guid pairingId,
            Guid tournamentParticipantId,
            Team team,
            Guid enteredBy)
        {
            return new PairingParticipant(pairingId, tournamentParticipantId, team, enteredBy);
        }

        public static Game CreateGame(Guid pairingId, int gameNumber = 1)
        {
            return new Game(pairingId, gameNumber, matchBonusEnabled: true);
        }

        public static Game CreateCompletedGame(Guid pairingId, Guid enteredByUserId, int gameNumber = 1)
        {
            var game = CreateGame(pairingId, gameNumber);
            game.SetScore(new GameScore(
                teamAPoints: 100,
                teamBPoints: 57,
                teamAMatchBonusAchieved: false,
                teamBMatchBonusAchieved: false,
                enteredByUserId));

            return game;
        }
    }
}
